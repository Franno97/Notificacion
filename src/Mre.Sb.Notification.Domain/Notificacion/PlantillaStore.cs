using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Mre.Sb.Notificacion
{

    public class PlantillaStore : IPlantillaStore, ITransientDependency
    {
        protected IDistributedCache<PlantillaCacheItem> Cache { get; }
        protected IPlantillaRepository PlantillaRepository { get; }

        public PlantillaStore(
         IPlantillaRepository plantillaRepository,
         IDistributedCache<PlantillaCacheItem> cache)
        {
            PlantillaRepository = plantillaRepository; 
            Cache = cache; 
        }


        public async Task<Plantilla> ObtenerAsync(string templateId)
        {
             
            var templateCacheItem = await ObtenerItemCacheAsync(templateId);
            if (templateCacheItem == null)
                return null;

            var template = new Plantilla(templateCacheItem.Id);

            template.Descripcion = templateCacheItem.Descripcion;
            template.Contenido = templateCacheItem.Contenido;

            return template;            

        }


        public async Task<List<Plantilla>> ObtenerListaAsync(string channelId)
        {
            var templates = await PlantillaRepository.ObtenerListaAsync(channelId);
            return templates;
        }

        #region Cache

        protected virtual async Task<PlantillaCacheItem> ObtenerItemCacheAsync(string templateId)
        {
            var cacheKey = PlantillaCacheItem.CalcularClaveCache(templateId);
            var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);

            if (cacheItem != null)
            {
                return cacheItem;
            }

          
            cacheItem = await EstablecerItemCacheAsync(templateId, cacheKey);

            return cacheItem;
        }


        private async Task<PlantillaCacheItem> EstablecerItemCacheAsync(
            string templateId,
            string cacheKey)
        {
            var template = await PlantillaRepository.GetAsync(templateId);

            if (template == null)
                return null;

            var templateCacheItem = new PlantillaCacheItem();
            templateCacheItem.Id = template.Id;
            templateCacheItem.Contenido = template.Contenido;
            templateCacheItem.Descripcion = template.Descripcion;

            await Cache.SetAsync(cacheKey,templateCacheItem, considerUow: true);

            return templateCacheItem;
        }

        #endregion Cache
    }
}
