using System.ComponentModel.DataAnnotations;

namespace Practos3.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "¬ведите логин.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "¬ведите пароль.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "ѕароль должен быть не менее 6 символов.")]
    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessage = "ѕароли не совпадают.")]
    public string ConfirmPassword { get; set; } = null!;

    public string? DisplayName { get; set; }

    public int RoleID { get; set; }
}
