using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class CreateCartDto
{
    [Required, Range(1, int.MaxValue)]
    public required int UserId { get; init; }
}
