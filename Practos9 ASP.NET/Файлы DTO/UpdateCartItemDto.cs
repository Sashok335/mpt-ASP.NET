using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class UpdateCartItemDto
{
    [Required, Range(1, int.MaxValue)]
    public required int CartId { get; init; }

    [Required, Range(1, int.MaxValue)]
    public required int ProductId { get; init; }

    [Required, Range(1, int.MaxValue)]
    public required int Quantity { get; init; }
}
