using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class CreateCategoryDto
{
    [Required, StringLength(100)]
    public required string CategoryName { get; init; }
}
