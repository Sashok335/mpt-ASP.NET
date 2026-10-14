using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchCategoryDto
{
    [StringLength(100, MinimumLength = 1), RegularExpression(@"[\s\S]*\S[\s\S]*")]
    public string? CategoryName { get; init; }
}
