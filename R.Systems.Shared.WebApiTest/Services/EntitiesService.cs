using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.WebApiTest.Models;

namespace R.Systems.Shared.WebApiTest.Services
{
    public class EntitiesService : IDependencyInjectionScoped
    {
        public List<Entity> Entities { get; } = new()
        {
            new Entity { Id = 1L, Name = "Entity 1" },
            new Entity { Id = 2L, Name = "Entity 2" },
            new Entity { Id = 3L, Name = "Entity 3" }
        };
    }
}
