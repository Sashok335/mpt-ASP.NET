using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchOrderDto
{
    [Range(1, int.MaxValue)]
    public int? UserId { get; init; }

    [StringLength(20, MinimumLength = 1), RegularExpression(@"[\s\S]*\S[\s\S]*")]
    public string? Status { get; init; }

    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public decimal? TotalAmount { get; init; }
}
