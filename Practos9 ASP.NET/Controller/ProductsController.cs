using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : CrudController<AdminProduct, ProductDto, CreateProductDto, UpdateProductDto, PatchProductDto>
{
    public ProductsController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminProduct, int>> Key => entity => entity.ProductID;

    protected override int GetId(AdminProduct entity) => entity.ProductID;

    protected override ProductDto MapToDto(AdminProduct entity) => ToDto(entity);

    private static ProductDto ToDto(AdminProduct entity) =>
        new(entity.ProductID, entity.ProductName, entity.Price, entity.CategoryID);

    protected override AdminProduct CreateEntity(CreateProductDto dto) => new()
    {
        ProductName = dto.ProductName,
        Price = dto.Price,
        CategoryID = dto.CategoryId
    };

    protected override void ApplyUpdate(AdminProduct entity, UpdateProductDto dto)
    {
        entity.ProductName = dto.ProductName;
        entity.Price = dto.Price;
        entity.CategoryID = dto.CategoryId;
    }

    protected override void ApplyPatch(AdminProduct entity, PatchProductDto dto)
    {
        entity.ProductName = dto.ProductName ?? entity.ProductName;
        entity.Price = dto.Price ?? entity.Price;
        entity.CategoryID = dto.CategoryId ?? entity.CategoryID;
    }

    protected override async Task<bool> ReferencesExistAsync(AdminProduct entity, CancellationToken cancellationToken) =>
        await ReferenceExistsAsync<AdminCategory>(entity.CategoryID, cancellationToken);
}
