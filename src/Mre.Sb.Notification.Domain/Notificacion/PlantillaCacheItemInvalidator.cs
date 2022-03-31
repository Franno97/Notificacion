using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Mre.Sb.Notificacion
{
    //public class PlantillaCacheItemInvalidator : ILocalEventHandler<EntityChangedEventData<Plantilla>>, ITransientDependency
    //{
    //    protected IDistributedCache<PlantillaCacheItem> Cache { get; }

    //    public PlantillaCacheItemInvalidator(IDistributedCache<PlantillaCacheItem> cache)
    //    {
    //        Cache = cache;
    //    }

    //    public virtual async Task HandleEventAsync(EntityChangedEventData<Plantilla> eventData)
    //    {
    //        var cacheKey = PlantillaCacheItem.CalcularClaveCache(eventData.Entity.Id);

    //        await Cache.RemoveAsync(cacheKey, considerUow: true);
    //    }


    //}

}
