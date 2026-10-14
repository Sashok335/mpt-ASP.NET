using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchCartItemDto
{
    [Range(1, int.MaxValue)]
    public int? CartId { get; init; }

    [Range(1, int.MaxValue)]
    public int? ProductId { get; init; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; init; }
}
