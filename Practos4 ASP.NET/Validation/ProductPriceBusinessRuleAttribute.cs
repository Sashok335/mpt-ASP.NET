using System.ComponentModel.DataAnnotations;
using Practos3.Models;

namespace Practos3.Validation;

public class ProductPriceBusinessRuleAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not decimal price)
        {
            return ValidationResult.Success;
        }

        var product = validationContext.ObjectInstance as AdminProduct;
        if (product == null)
        {
            return ValidationResult.Success;
        }

        if (price < 100 || price > 500000)
        {
            return new ValidationResult(ErrorMessage ?? "Цена должна быть в диапазоне от 100 до 500000.");
        }

        if (decimal.Round(price, 2) != price)
        {
            return new ValidationResult(ErrorMessage ?? "Цена может содержать не более 2 знаков после запятой.");
        }

        if (product.CategoryID == 1 && price < 1000)
        {
            return new ValidationResult(ErrorMessage ?? "Для категории смартфонов цена должна быть не меньше 1000.");
        }

        if (product.CategoryID == 4 && price > 100000)
        {
            return new ValidationResult(ErrorMessage ?? "Для аудиотоваров цена выглядит нереалистично высокой.");
        }

        return ValidationResult.Success;
    }
}
