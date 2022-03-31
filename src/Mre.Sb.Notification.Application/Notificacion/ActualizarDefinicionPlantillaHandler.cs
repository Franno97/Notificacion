using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.TextTemplating;

namespace Mre.Sb.Notificacion.Application
{
    public class ActualizarDefinicionPlantillaHandler :  
          IDistributedEventHandler<EntityCreatedEto<PlantillaEto>>
        , IDistributedEventHandler<EntityDeletedEto<PlantillaEto>>
        , IDistributedEventHandler<EntityUpdatedEto<PlantillaEto>>
        , ITransientDependency

    {

        public ITemplateDefinitionManager TemplateDefinitionManager { get; }
        protected IDistributedCache<PlantillaCacheItem> Cache { get; }

        public ILogger<ActualizarDefinicionPlantillaHandler> Logger { get; set; }

        public ActualizarDefinicionPlantillaHandler(
            ITemplateDefinitionManager templateDefinitionManager,
            IDistributedCache<PlantillaCacheItem> cache)
        {
            TemplateDefinitionManager = templateDefinitionManager;
            Cache = cache;
            Logger = NullLogger<ActualizarDefinicionPlantillaHandler>.Instance;
        }

       
       
        public virtual async Task HandleEventAsync(EntityCreatedEto<PlantillaEto> eventData)
        {
            Logger.LogInformation($"Procesar evento creacion. Id: {eventData.Entity.Id}");


            var gestionDefinicionPlantilla = TemplateDefinitionManager as GestionDefinicionPlantilla;

            if (gestionDefinicionPlantilla != null)
            {
                var plantillaDefinicionExistente = gestionDefinicionPlantilla.Get(eventData.Entity.Id.ToUpper());

                if (plantillaDefinicionExistente == null)
                {

                    Logger.LogInformation($"Agregar definicion plantilla. Id: {eventData.Entity.Id.ToUpper()}");

                    var templateDefinition = new TemplateDefinition(eventData.Entity.Id.ToUpper());

                    templateDefinition.WithProperty(
                        DatabaseTemplateContentContributor.DatabasePropertyName,
                        eventData.Entity.Id.ToUpper()
                    );

                    gestionDefinicionPlantilla.Add(eventData.Entity.Id.ToUpper(), templateDefinition);
                } 
            }
             
            await LimpiarChache(eventData.Entity);
        }

        
        public virtual async Task HandleEventAsync(EntityDeletedEto<PlantillaEto> eventData)
        {
            Logger.LogInformation($"Procesar evento eliminacion. Id: {eventData.Entity.Id}");

            var gestionDefinicionPlantilla = TemplateDefinitionManager as GestionDefinicionPlantilla;

            if (gestionDefinicionPlantilla != null)
            {
                Logger.LogInformation($"Eliminar definicion plantilla. Id: {eventData.Entity.Id.ToUpper()}");


                gestionDefinicionPlantilla.Delete(eventData.Entity.Id.ToUpper());
            }

            await LimpiarChache(eventData.Entity);
        }

        public virtual async Task HandleEventAsync(EntityUpdatedEto<PlantillaEto> eventData)
        {
            Logger.LogInformation($"Procesar evento actualizacion. Id: {eventData.Entity.Id}");

            await LimpiarChache(eventData.Entity);
        }


        private async Task LimpiarChache(PlantillaEto eventData)
        {
            var cacheKey = PlantillaCacheItem.CalcularClaveCache(eventData.Id);

            await Cache.RemoveAsync(cacheKey, considerUow: true);
        }


    }



}
