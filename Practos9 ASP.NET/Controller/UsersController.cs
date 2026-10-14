using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : CrudController<AdminUser, UserDto, CreateUserDto, UpdateUserDto, PatchUserDto>
{
    public UsersController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminUser, int>> Key => entity => entity.UserID;

    protected override int GetId(AdminUser entity) => entity.UserID;

    protected override UserDto MapToDto(AdminUser entity) => ToDto(entity);

    private static UserDto ToDto(AdminUser entity) =>
        new(entity.UserID, entity.Login, entity.RoleID);

    protected override AdminUser CreateEntity(CreateUserDto dto) => new()
    {
        Login = dto.Login,
        PasswordHash = PasswordHash.Create(dto.Password),
        RoleID = dto.RoleId
    };

    protected override void ApplyUpdate(AdminUser entity, UpdateUserDto dto)
    {
        entity.Login = dto.Login;
        entity.PasswordHash = PasswordHash.Create(dto.Password);
        entity.RoleID = dto.RoleId;
    }

    protected override void ApplyPatch(AdminUser entity, PatchUserDto dto)
    {
        entity.Login = dto.Login ?? entity.Login;
        if (dto.Password is { } password)
            entity.PasswordHash = PasswordHash.Create(password);
        entity.RoleID = dto.RoleId ?? entity.RoleID;
    }

    protected override async Task<bool> ReferencesExistAsync(AdminUser entity, CancellationToken cancellationToken) =>
        await ReferenceExistsAsync<AdminRole>(entity.RoleID, cancellationToken);
}
