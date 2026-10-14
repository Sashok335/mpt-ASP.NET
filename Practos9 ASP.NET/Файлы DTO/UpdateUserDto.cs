using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class UpdateUserDto
{
    [Required, StringLength(100)]
    public required string Login { get; init; }

    [Required, StringLength(128, MinimumLength = 8)]
    public required string Password { get; init; }

    [Required, Range(1, int.MaxValue)]
    public required int RoleId { get; init; }
}
