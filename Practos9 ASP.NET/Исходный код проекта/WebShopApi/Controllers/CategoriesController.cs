using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController : CrudController<AdminCategory, CategoryDto, CreateCategoryDto, UpdateCategoryDto, PatchCategoryDto>
{
    public CategoriesController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminCategory, int>> Key => entity => entity.CategoryID;

    protected override int GetId(AdminCategory entity) => entity.CategoryID;

    protected override CategoryDto MapToDto(AdminCategory entity) => ToDto(entity);

    private static CategoryDto ToDto(AdminCategory entity) =>
        new(entity.CategoryID, entity.CategoryName);

    protected override AdminCategory CreateEntity(CreateCategoryDto dto) => new()
    {
        CategoryName = dto.CategoryName
    };

    protected override void ApplyUpdate(AdminCategory entity, UpdateCategoryDto dto)
    {
        entity.CategoryName = dto.CategoryName;
    }

    protected override void ApplyPatch(AdminCategory entity, PatchCategoryDto dto)
    {
        entity.CategoryName = dto.CategoryName ?? entity.CategoryName;
    }
}
