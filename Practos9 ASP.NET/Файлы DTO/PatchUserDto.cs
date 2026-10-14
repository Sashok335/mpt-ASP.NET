using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchUserDto
{
    [StringLength(100, MinimumLength = 1), RegularExpression(@"[\s\S]*\S[\s\S]*")]
    public string? Login { get; init; }

    [StringLength(128, MinimumLength = 8), RegularExpression(@"[\s\S]*\S[\s\S]*")]
    public string? Password { get; init; }

    [Range(1, int.MaxValue)]
    public int? RoleId { get; init; }
}
