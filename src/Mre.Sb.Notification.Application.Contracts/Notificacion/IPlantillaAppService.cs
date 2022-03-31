using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Mre.Sb.Notificacion
{
    public interface IPlantillaAppService :
      ICrudAppService< 
          PlantillaDto, 
          string, 
          PagedAndSortedResultRequestDto, 
          CrearActualizarPlantillaDto> 
    {

    }
}
