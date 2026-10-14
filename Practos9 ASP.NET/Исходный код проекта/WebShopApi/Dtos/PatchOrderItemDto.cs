using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchOrderItemDto
{
    [Range(1, int.MaxValue)]
    public int? OrderId { get; init; }

    [Range(1, int.MaxValue)]
    public int? ProductId { get; init; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; init; }

    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public decimal? UnitPrice { get; init; }
}
