using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/cartitems")]
public sealed class CartItemsController : CrudController<AdminCartItem, CartItemDto, CreateCartItemDto, UpdateCartItemDto, PatchCartItemDto>
{
    public CartItemsController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminCartItem, int>> Key => entity => entity.CartItemID;

    protected override int GetId(AdminCartItem entity) => entity.CartItemID;

    protected override CartItemDto MapToDto(AdminCartItem entity) => ToDto(entity);

    private static CartItemDto ToDto(AdminCartItem entity) =>
        new(entity.CartItemID, entity.CartID, entity.ProductID, entity.Quantity);

    protected override AdminCartItem CreateEntity(CreateCartItemDto dto) => new()
    {
        CartID = dto.CartId,
        ProductID = dto.ProductId,
        Quantity = dto.Quantity
    };

    protected override void ApplyUpdate(AdminCartItem entity, UpdateCartItemDto dto)
    {
        entity.CartID = dto.CartId;
        entity.ProductID = dto.ProductId;
        entity.Quantity = dto.Quantity;
    }

    protected override void ApplyPatch(AdminCartItem entity, PatchCartItemDto dto)
    {
        entity.CartID = dto.CartId ?? entity.CartID;
        entity.ProductID = dto.ProductId ?? entity.ProductID;
        entity.Quantity = dto.Quantity ?? entity.Quantity;
    }

    protected override async Task<bool> ReferencesExistAsync(AdminCartItem entity, CancellationToken cancellationToken) =>
        await ReferenceExistsAsync<AdminCart>(entity.CartID, cancellationToken) &&
        await ReferenceExistsAsync<AdminProduct>(entity.ProductID, cancellationToken);
}
