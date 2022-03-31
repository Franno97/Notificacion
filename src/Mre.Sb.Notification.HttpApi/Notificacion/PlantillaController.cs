using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.Notificacion.HttpApi
{
    [RemoteService(Name = "notificacion")]
    [Area("notificacion")]
    [Route("api/notificacion/plantilla")]
    [Authorize]
    public class PlantillaController : NotificacionBaseController, IPlantillaAppService
    {
        public IPlantillaAppService TemplateAppService { get; }

        public PlantillaController(IPlantillaAppService templateAppService)
        {
            TemplateAppService = templateAppService;
        }


        [HttpGet]
        public virtual Task<PagedResultDto<PlantillaDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return TemplateAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PlantillaDto> GetAsync(string id)
        {
            return TemplateAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<PlantillaDto> CreateAsync(CrearActualizarPlantillaDto input)
        {
            return TemplateAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PlantillaDto> UpdateAsync(string id, CrearActualizarPlantillaDto input)
        {
            return TemplateAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(string id)
        {
            return TemplateAppService.DeleteAsync(id);
        }
          
        
    }
}
