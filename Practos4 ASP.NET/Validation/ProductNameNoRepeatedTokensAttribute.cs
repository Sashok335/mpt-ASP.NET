using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Practos3.Validation;

public class ProductNameNoRepeatedTokensAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var text = value as string;
        if (string.IsNullOrWhiteSpace(text))
        {
            return ValidationResult.Success;
        }

        var tokens = Regex.Split(text.Trim(), @"\s+")
            .Where(token => !string.IsNullOrWhiteSpace(token))
            .Select(token => token.ToLowerInvariant())
            .ToList();

        var hasSpamRepeat = tokens
            .GroupBy(token => token)
            .Any(group => group.Count() >= 3);

        if (hasSpamRepeat)
        {
            return new ValidationResult(ErrorMessage ?? "Название не должно содержать одно и то же слово 3 раза и более.");
        }

        return ValidationResult.Success;
    }
}
