using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.Core.Validation;
using R.Systems.Shared.WebApiTest.Models;
using R.Systems.Shared.WebApiTest.Services;

namespace R.Systems.Shared.WebApiTest.Validation
{
    public class EntityValidator : IDependencyInjectionScoped
    {
        public EntityValidator(ValidationResult validationResult, EntitiesService entitiesService)
        {
            ValidationResult = validationResult;
            EntitiesService = entitiesService;
        }

        public ValidationResult ValidationResult { get; }
        public EntitiesService EntitiesService { get; }

        public bool Validate(Entity entity, long? entityId = null)
        {
            bool result = true;
            result &= ValidateEntityId(entityId);
            result &= ValidateEntityName(entity.Name);
            return result;
        }

        private bool ValidateEntityId(long? entityId = null)
        {
            if (entityId == null)
            {
                return true;
            }
            Entity? foundEntity = EntitiesService.Entities.Where(x => x.Id == entityId).FirstOrDefault();
            if (foundEntity == null)
            {
                ValidationResult.Errors.Add(new ErrorInfo(errorKey: "NotExist", elementKey: "EntityId"));
                return false;
            }
            return true;
        }

        private bool ValidateEntityName(string entityName)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                ValidationResult.Errors.Add(new ErrorInfo(errorKey: "IsRequired", elementKey: "EntityName"));
                return false;
            }
            return true;
        }
    }
}
