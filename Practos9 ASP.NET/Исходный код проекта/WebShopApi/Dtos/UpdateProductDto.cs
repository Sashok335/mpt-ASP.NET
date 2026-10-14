using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class UpdateProductDto
{
    [Required, StringLength(200)]
    public required string ProductName { get; init; }

    [Required, Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public required decimal Price { get; init; }

    [Required, Range(1, int.MaxValue)]
    public required int CategoryId { get; init; }
}
