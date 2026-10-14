using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/orderitems")]
public sealed class OrderItemsController : CrudController<AdminOrderItem, OrderItemDto, CreateOrderItemDto, UpdateOrderItemDto, PatchOrderItemDto>
{
    public OrderItemsController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminOrderItem, int>> Key => entity => entity.OrderItemID;

    protected override int GetId(AdminOrderItem entity) => entity.OrderItemID;

    protected override OrderItemDto MapToDto(AdminOrderItem entity) => ToDto(entity);

    private static OrderItemDto ToDto(AdminOrderItem entity) =>
        new(entity.OrderItemID, entity.OrderID, entity.ProductID, entity.Quantity, entity.UnitPrice);

    protected override AdminOrderItem CreateEntity(CreateOrderItemDto dto) => new()
    {
        OrderID = dto.OrderId,
        ProductID = dto.ProductId,
        Quantity = dto.Quantity,
        UnitPrice = dto.UnitPrice
    };

    protected override void ApplyUpdate(AdminOrderItem entity, UpdateOrderItemDto dto)
    {
        entity.OrderID = dto.OrderId;
        entity.ProductID = dto.ProductId;
        entity.Quantity = dto.Quantity;
        entity.UnitPrice = dto.UnitPrice;
    }

    protected override void ApplyPatch(AdminOrderItem entity, PatchOrderItemDto dto)
    {
        entity.OrderID = dto.OrderId ?? entity.OrderID;
        entity.ProductID = dto.ProductId ?? entity.ProductID;
        entity.Quantity = dto.Quantity ?? entity.Quantity;
        entity.UnitPrice = dto.UnitPrice ?? entity.UnitPrice;
    }

    protected override async Task<bool> ReferencesExistAsync(AdminOrderItem entity, CancellationToken cancellationToken) =>
        await ReferenceExistsAsync<AdminOrder>(entity.OrderID, cancellationToken) &&
        await ReferenceExistsAsync<AdminProduct>(entity.ProductID, cancellationToken);
}
