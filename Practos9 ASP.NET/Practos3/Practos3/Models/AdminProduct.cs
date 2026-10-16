using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Practos3.Validation;

namespace Practos3.Models;

public class AdminProduct
{
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Введите название товара.")]
    [StringLength(200, MinimumLength = 4, ErrorMessage = "Название должно содержать от 4 до 200 символов.")]
    [ProductNameHasLetters(ErrorMessage = "В названии должно быть минимум 3 буквы.")]
    [ProductNameNoForbiddenChars(ErrorMessage = "Название содержит недопустимые символы.")]
    [ProductNameNoRepeatedTokens(ErrorMessage = "Название не должно повторять одно и то же слово 3 раза и более.")]
    [ProductNameMeaningfulStructure(ErrorMessage = "Название должно быть осмысленным и без двойных пробелов.")]
    public string ProductName { get; set; } = null!;

    [ProductPriceBusinessRule(ErrorMessage = "Цена не проходит бизнес-проверку.")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Выберите категорию.")]
    public int CategoryID { get; set; }

    [ValidateNever]
    public AdminCategory? Category { get; set; }

    [ValidateNever]
    public ICollection<AdminCartItem> CartItems { get; set; } = new List<AdminCartItem>();

    [ValidateNever]
    public ICollection<AdminOrderItem> OrderItems { get; set; } = new List<AdminOrderItem>();
}
