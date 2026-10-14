using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchProductDto
{
    [StringLength(200, MinimumLength = 1), RegularExpression(@"[\s\S]*\S[\s\S]*")]
    public string? ProductName { get; init; }

    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public decimal? Price { get; init; }

    [Range(1, int.MaxValue)]
    public int? CategoryId { get; init; }
}
