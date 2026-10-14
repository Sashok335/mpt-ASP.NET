namespace WebShopApi.Dtos;

public sealed record OrderItemDto(int Id, int OrderId, int ProductId, int Quantity, decimal UnitPrice);
