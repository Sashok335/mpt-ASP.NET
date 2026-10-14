namespace WebShopApi.Dtos;

public sealed record OrderDto(int Id, int UserId, DateTime? CreatedAt, string? Status, decimal TotalAmount);
