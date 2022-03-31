using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.Notificacion.Application
{
    public class PlantillaAppService :
       CrudAppService<Plantilla,
          PlantillaDto,
          string,
          PagedAndSortedResultRequestDto,
          CrearActualizarPlantillaDto>,
        IPlantillaAppService
    {
        public PlantillaManager PlantillaManager { get; }

        public PlantillaAppService(
            IPlantillaRepository repository,
            PlantillaManager plantillaManager)
            : base(repository)
        {
            PlantillaManager = plantillaManager;
        }


        public override async Task<PlantillaDto> CreateAsync(CrearActualizarPlantillaDto input)
        {
            await CheckCreatePolicyAsync();

            await PlantillaManager.ValidarCrearAsync(input.Id);

            var entity = new Plantilla(input.Id);

            await MapToEntityAsync(input, entity);

            TryToSetTenantId(entity);

            entity = await PlantillaManager.InsertAsync(entity, autoSave: true);

            return await MapToGetOutputDtoAsync(entity);
        }
    }

  
  
}
