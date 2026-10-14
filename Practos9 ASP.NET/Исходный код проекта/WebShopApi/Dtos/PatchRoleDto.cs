using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchRoleDto
{
    [StringLength(50, MinimumLength = 1), RegularExpression(@"[\s\S]*\S[\s\S]*")]
    public string? RoleName { get; init; }
}
