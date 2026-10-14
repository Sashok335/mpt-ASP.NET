using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WebShopApi.Data;
using WebShopApi.Dtos;
using WebShopApi.Models;

namespace WebShopApi.Controllers;

[ApiController]
[Route("api/roles")]
public sealed class RolesController : CrudController<AdminRole, RoleDto, CreateRoleDto, UpdateRoleDto, PatchRoleDto>
{
    public RolesController(WebShopDbContext context) : base(context)
    {
    }

    protected override Expression<Func<AdminRole, int>> Key => entity => entity.RoleID;

    protected override int GetId(AdminRole entity) => entity.RoleID;

    protected override RoleDto MapToDto(AdminRole entity) => ToDto(entity);

    private static RoleDto ToDto(AdminRole entity) =>
        new(entity.RoleID, entity.RoleName);

    protected override AdminRole CreateEntity(CreateRoleDto dto) => new()
    {
        RoleName = dto.RoleName
    };

    protected override void ApplyUpdate(AdminRole entity, UpdateRoleDto dto)
    {
        entity.RoleName = dto.RoleName;
    }

    protected override void ApplyPatch(AdminRole entity, PatchRoleDto dto)
    {
        entity.RoleName = dto.RoleName ?? entity.RoleName;
    }
}
