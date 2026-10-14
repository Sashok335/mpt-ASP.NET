namespace WebShopApi.Dtos;

public sealed record ProductDto(int Id, string ProductName, decimal Price, int CategoryId);
