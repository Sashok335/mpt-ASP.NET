using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : CrudController<AdminOrder, OrderDto, CreateOrderDto, UpdateOrderDto, PatchOrderDto>
{
    public OrdersController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminOrder, int>> Key => entity => entity.OrderID;

    protected override int GetId(AdminOrder entity) => entity.OrderID;

    protected override OrderDto MapToDto(AdminOrder entity) => ToDto(entity);

    private static OrderDto ToDto(AdminOrder entity) =>
        new(entity.OrderID, entity.UserID, entity.OrderDate, entity.Status, entity.TotalAmount);

    protected override AdminOrder CreateEntity(CreateOrderDto dto) => new()
    {
        UserID = dto.UserId,
        Status = dto.Status,
        TotalAmount = dto.TotalAmount,
        OrderDate = DateTime.UtcNow
    };

    protected override void ApplyUpdate(AdminOrder entity, UpdateOrderDto dto)
    {
        entity.UserID = dto.UserId;
        entity.Status = dto.Status;
        entity.TotalAmount = dto.TotalAmount;
    }

    protected override void ApplyPatch(AdminOrder entity, PatchOrderDto dto)
    {
        entity.UserID = dto.UserId ?? entity.UserID;
        entity.Status = dto.Status ?? entity.Status;
        entity.TotalAmount = dto.TotalAmount ?? entity.TotalAmount;
    }

    protected override async Task<bool> ReferencesExistAsync(AdminOrder entity, CancellationToken cancellationToken) =>
        await ReferenceExistsAsync<AdminUser>(entity.UserID, cancellationToken);
}
