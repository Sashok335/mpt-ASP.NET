using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class CreateOrderItemDto
{
    [Required, Range(1, int.MaxValue)]
    public required int OrderId { get; init; }

    [Required, Range(1, int.MaxValue)]
    public required int ProductId { get; init; }

    [Required, Range(1, int.MaxValue)]
    public required int Quantity { get; init; }

    [Required, Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public required decimal UnitPrice { get; init; }
}
