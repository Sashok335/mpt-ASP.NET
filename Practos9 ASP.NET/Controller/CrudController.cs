using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShopApi.Data;

namespace WebShopApi.Controllers;

public abstract class CrudController<TEntity, TDto, TCreate, TUpdate, TPatch> : ControllerBase
    where TEntity : class
{
    protected WebShopDbContext Context { get; }

    protected CrudController(WebShopDbContext context)
    {
        Context = context;
    }

    private DbSet<TEntity> Entities => Context.Set<TEntity>();

    protected abstract Expression<Func<TEntity, int>> Key { get; }
    protected abstract int GetId(TEntity entity);
    protected abstract TDto MapToDto(TEntity entity);
    protected abstract TEntity CreateEntity(TCreate dto);
    protected abstract void ApplyUpdate(TEntity entity, TUpdate dto);
    protected abstract void ApplyPatch(TEntity entity, TPatch dto);

    protected virtual Task<bool> ReferencesExistAsync(TEntity entity, CancellationToken cancellationToken) =>
        Task.FromResult(true);

    protected async Task<bool> ReferenceExistsAsync<TReference>(int id, CancellationToken cancellationToken)
        where TReference : class =>
        await Context.Set<TReference>().FindAsync([id], cancellationToken) is not null;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<TDto>>> GetAll(
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, int.MaxValue)] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var offset = ((long)page - 1) * pageSize;
        if (offset > int.MaxValue)
        {
            ModelState.AddModelError(nameof(page), "The requested page exceeds the supported offset.");
            return ValidationProblem(ModelState);
        }

        var entities = await Entities.AsNoTracking()
            .OrderBy(Key)
            .Skip((int)offset)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Ok(entities.Select(MapToDto).ToList());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await Entities.FindAsync([id], cancellationToken);
        return entity is null ? NotFound() : Ok(MapToDto(entity));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TDto>> Create(TCreate dto, CancellationToken cancellationToken)
    {
        var entity = CreateEntity(dto);
        if (!await ReferencesExistAsync(entity, cancellationToken))
            return NotFound();

        Entities.Add(entity);
        try
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsConstraintViolation(ex))
        {
            return Conflict();
        }

        await Context.Entry(entity).ReloadAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = GetId(entity) }, MapToDto(entity));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, TUpdate dto, CancellationToken cancellationToken)
    {
        var entity = await Entities.FindAsync([id], cancellationToken);
        if (entity is null)
            return NotFound();

        ApplyUpdate(entity, dto);
        if (!await ReferencesExistAsync(entity, cancellationToken))
            return NotFound();

        return await SaveChangesAsync(cancellationToken);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(int id, TPatch dto, CancellationToken cancellationToken)
    {
        var entity = await Entities.FindAsync([id], cancellationToken);
        if (entity is null)
            return NotFound();

        ApplyPatch(entity, dto);
        if (!await ReferencesExistAsync(entity, cancellationToken))
            return NotFound();

        return await SaveChangesAsync(cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await Entities.FindAsync([id], cancellationToken);
        if (entity is null)
            return NotFound();

        Entities.Remove(entity);
        return await SaveChangesAsync(cancellationToken);
    }

    private async Task<IActionResult> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
        catch (DbUpdateException ex) when (IsConstraintViolation(ex))
        {
            return Conflict();
        }
    }

    private static bool IsConstraintViolation(DbUpdateException exception) =>
        exception.InnerException is SqlException { Number: 547 or 2601 or 2627 };
}
