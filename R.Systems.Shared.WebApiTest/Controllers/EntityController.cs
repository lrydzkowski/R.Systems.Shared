using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Shared.Core.Validation;
using R.Systems.Shared.WebApiTest.Models;
using R.Systems.Shared.WebApiTest.Services;
using R.Systems.Shared.WebApiTest.Validation;

namespace R.Systems.Shared.WebApiTest.Controllers;

[ApiController, Route("entities")]
public class EntityController : ControllerBase
{
    public EntityController(
        EntitiesService entitiesService,
        EntityValidator entityValidator,
        ValidationResult validationResult)
    {
        EntitiesService = entitiesService;
        EntityValidator = entityValidator;
        ValidationResult = validationResult;
    }

    public EntitiesService EntitiesService { get; }
    public EntityValidator EntityValidator { get; }
    public ValidationResult ValidationResult { get; }

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
        if (!EntityValidator.Validate(entity))
        {
            return BadRequest(ValidationResult.Errors);
        }
        return Ok();
    }

    [HttpPost, Route("{entityId}"), Authorize(Roles = "admin")]
    public IActionResult UpdateEntity(long entityId, Entity entity)
    {
        if (!EntityValidator.Validate(entity, entityId))
        {
            return BadRequest(ValidationResult.Errors);
        }
        return Ok();
    }

    [HttpDelete, Route("{entityId}"), Authorize(Roles = "admin")]
    public IActionResult DeleteEntity(long entityId)
    {
        return Ok();
    }
}
