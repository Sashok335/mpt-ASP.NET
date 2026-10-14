namespace WebShopApi.Dtos;

public sealed record CartItemDto(int Id, int CartId, int ProductId, int Quantity);
