using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos;

public sealed class PatchCartDto
{
    [Range(1, int.MaxValue)]
    public int? UserId { get; init; }
}
