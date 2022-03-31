using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp;

namespace Mre.Sb.Notificacion.HttpApi
{

    [RemoteService(Name = "notificacion")]
    [Area("notificacion")]
    [Route("api/notificacion/notificador")]
    [Authorize]
    public class NotificadorController : NotificacionBaseController, INotificadorAppService
    {

        public INotificadorAppService NotificacionAppService { get; }


        public NotificadorController(INotificadorAppService notificacionAppService)
        {
            NotificacionAppService = notificacionAppService;
        }

        [HttpPost]
        public async Task<bool> EnviarAsync(NotificacionMensajeDto notificationMessage) {

            return await NotificacionAppService.EnviarAsync(notificationMessage);
        }

        [HttpPost("enviar-con-adjuntos")]
        public async Task<bool> EnviarConAdjuntosAsync(
            IList<IFormFile> adjuntos,
            [FromForm] string codigo,
            [FromForm] string asunto,
            [FromForm] string destinatarios,
            [FromForm][ModelBinder(typeof(DictionaryBinder))] Dictionary<string, string> model)
        {
         
            return await NotificacionAppService.EnviarConAdjuntosAsync(adjuntos,
            codigo,asunto,destinatarios,model);
        }
    }

    public class DictionaryBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (bindingContext.HttpContext.Request.HasFormContentType)
            {
                var form = bindingContext.HttpContext.Request.Form;
                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(form[bindingContext.FieldName].ToString());
                bindingContext.Result = ModelBindingResult.Success(data);
            }
        }
    }

}
