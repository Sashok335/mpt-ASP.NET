using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/carts")]
public sealed class CartsController : CrudController<AdminCart, CartDto, CreateCartDto, UpdateCartDto, PatchCartDto>
{
    public CartsController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminCart, int>> Key => entity => entity.CartID;

    protected override int GetId(AdminCart entity) => entity.CartID;

    protected override CartDto MapToDto(AdminCart entity) => ToDto(entity);

    private static CartDto ToDto(AdminCart entity) =>
        new(entity.CartID, entity.UserID);

    protected override AdminCart CreateEntity(CreateCartDto dto) => new()
    {
        UserID = dto.UserId
    };

    protected override void ApplyUpdate(AdminCart entity, UpdateCartDto dto)
    {
        entity.UserID = dto.UserId;
    }

    protected override void ApplyPatch(AdminCart entity, PatchCartDto dto)
    {
        entity.UserID = dto.UserId ?? entity.UserID;
    }

    protected override async Task<bool> ReferencesExistAsync(AdminCart entity, CancellationToken cancellationToken) =>
        await ReferenceExistsAsync<AdminUser>(entity.UserID, cancellationToken);
}
