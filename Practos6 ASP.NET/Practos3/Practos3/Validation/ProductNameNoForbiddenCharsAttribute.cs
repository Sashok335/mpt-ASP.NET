using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Practos3.Validation;

public class ProductNameNoForbiddenCharsAttribute : ValidationAttribute
{
    private static readonly Regex AllowedPattern = new(@"^[A-Za-zА-Яа-я0-9\s\-\+\.,]+$", RegexOptions.Compiled);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var text = value as string;
        if (string.IsNullOrWhiteSpace(text))
        {
            return ValidationResult.Success;
        }

        if (!AllowedPattern.IsMatch(text))
        {
            return new ValidationResult("Название содержит недопустимые символы.");
        }

        return ValidationResult.Success;
    }
}
