using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Shared.WebApiTest.Models;
using R.Systems.Shared.WebApiTest.Services;

namespace R.Systems.Shared.WebApiTest.Controllers;

[ApiController, Route("entities")]
public class EntityController : ControllerBase
{
    public EntitiesService EntitiesService { get; }

    public EntityController(EntitiesService entitiesService)
    {
        EntitiesService = entitiesService;
    }

    [HttpGet, Authorize(Roles = "admin")]
    public IActionResult GetEntities()
    {
        List<Entity> entities = EntitiesService.Entities;
        return Ok(entities);
    }

    [HttpGet, Route("{entityId}"), Authorize(Roles = "admin")]
    public IActionResult GetEntity(long entityId)
    {
        Entity? entity = EntitiesService.Entities.Where(x => x.Id == entityId).FirstOrDefault();
        if (entity == null)
        {
            return NotFound(null);
        }
        return Ok(entity);
    }

    [HttpPost, Authorize(Roles = "admin")]
    public IActionResult CreateEntity(Entity entity)
    {
        return Ok();
    }

    [HttpPost, Route("{entityId}"), Authorize(Roles = "admin")]
    public IActionResult UpdateEntity(long entityId, Entity entity)
    {
        return Ok();
    }

    [HttpDelete, Authorize(Roles = "admin")]
    public IActionResult DeleteEntity(long entityId)
    {
        return Ok();
    }
}
