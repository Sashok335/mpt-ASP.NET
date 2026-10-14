using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class CreateRoleDto
{
    [Required, StringLength(50)]
    public required string RoleName { get; init; }
}
