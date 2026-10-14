using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class UpdateOrderDto
{
    [Required, Range(1, int.MaxValue)]
    public required int UserId { get; init; }

    [Required, StringLength(20)]
    public required string Status { get; init; }

    [Required, Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public required decimal TotalAmount { get; init; }
}
