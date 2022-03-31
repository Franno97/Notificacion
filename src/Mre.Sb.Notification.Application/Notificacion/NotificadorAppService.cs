using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mre.Sb.Notificacion.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Emailing;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.TextTemplating;

namespace Mre.Sb.Notificacion.Application
{

    public class NotificadorAppService : NotificacionBaseAppService, INotificadorAppService
    {
        private readonly ITemplateRenderer templateRenderer;
        private readonly ITemplateDefinitionManager templateDefinitionManager;
        private readonly IDistributedEventBus distributedEventBus;

        private readonly IStringLocalizer<NotificationResource> localizer;
        private readonly IEmailSender emailSender;
        private readonly IEmailSenderConfiguration configuracionEnvioEmail;
        private readonly ILogger<NotificadorAppService> logger;
        private readonly NotificacionConfiguracion  notificacionConfiguracion;

        public NotificadorAppService(
            ITemplateRenderer templateRenderer,
            ITemplateDefinitionManager templateDefinitionManager,
            IDistributedEventBus distributedEventBus,
            IStringLocalizer<NotificationResource> localizer,
            IEmailSender emailSender,
            IEmailSenderConfiguration configuracionEnvioEmail,
            IOptions<NotificacionConfiguracion> notificacionConfiguracionOptions,
            ILogger<NotificadorAppService> logger)
        {
            this.templateRenderer = templateRenderer;
            this.templateDefinitionManager = templateDefinitionManager;
            this.distributedEventBus = distributedEventBus;
            this.localizer = localizer;
            this.emailSender = emailSender;
            this.configuracionEnvioEmail = configuracionEnvioEmail;
            this.logger = logger;
            notificacionConfiguracion = notificacionConfiguracionOptions.Value;
        }

    
        public async Task<bool> EnviarAsync(NotificacionMensajeDto notificationMessage)
        {

            logger.LogInformation("Enviar notificacion. Codigo Plantilla {plantillaCodigo}", notificationMessage.Codigo);

            var result = false;

            var templateDefinition = templateDefinitionManager.GetOrNull(notificationMessage.Codigo.ToUpper());

            if (templateDefinition == null)
            {
                var msg = string.Format(localizer["Plantilla:NoExiste"], notificationMessage.Codigo.ToUpper());
                Logger.Log(LogLevel.Error, msg);
                return result;
            }

             
            if (!string.IsNullOrEmpty(notificacionConfiguracion.DestinatariosReemplazar))
            {

                logger.LogInformation("Reemplazar destinos.  Codigo Plantilla {plantillaCodigo}. Correos Destinos: {correosDestinos} Correos Reemplazar {correosDestinosFijos}", notificationMessage.Codigo, notificationMessage.Destinatarios, notificacionConfiguracion.DestinatariosReemplazar);

                notificationMessage.Destinatarios = notificacionConfiguracion.DestinatariosReemplazar;
            }


            if (notificacionConfiguracion.ProcesarAsincronamente)
            {
                result = await ProcesamientoAsincronoAsync(notificationMessage);
            }
            else {
                result = await ProcesamientoDirectoAsync(notificationMessage);

            }

            return result;
        }

        
        public async Task<bool> EnviarConAdjuntosAsync(IList<IFormFile> adjuntos, string codigo, string asunto, string destinatarios, Dictionary<string, string> model)
        {

            logger.LogInformation("Enviar notificacion Con Adjuntos. Codigo Plantilla {plantillaCodigo}", codigo);


            var result = false;
            if (adjuntos == null || adjuntos.Count == 0) {
                throw new UserFriendlyException(localizer["Notificador:Validacion:SinAdjuntos"]);
            }

            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new UserFriendlyException(localizer["Notificador:Validacion:Codigo"]);
            }

            if (string.IsNullOrWhiteSpace(asunto))
            {
                throw new UserFriendlyException(localizer["Notificador:Validacion:Asunto"]);
            }

            if (string.IsNullOrWhiteSpace(destinatarios))
            {
                throw new UserFriendlyException(localizer["Notificador:Validacion:Destinatarios"]);
            }

            var templateDefinition = templateDefinitionManager.GetOrNull(codigo.ToUpper());

            if (templateDefinition == null)
            {
                var msg = string.Format(localizer["Plantilla:NoExiste"], codigo.ToUpper());
                Logger.Log(LogLevel.Error, msg);
                return result;
            }


            if (!string.IsNullOrEmpty(notificacionConfiguracion.DestinatariosReemplazar))
            {
                logger.LogInformation("Reemplazar destinos.  Codigo Plantilla {plantillaCodigo}. Correos Destinos: {correosDestinos} Correos Reemplazar {correosDestinosFijos}", codigo, destinatarios, notificacionConfiguracion.DestinatariosReemplazar);

                destinatarios = notificacionConfiguracion.DestinatariosReemplazar;
            }

            var mensajeConAdjuntosDto = new NotificacionMensajeConAdjuntosDto();
            mensajeConAdjuntosDto.Codigo = codigo;
            mensajeConAdjuntosDto.Asunto = asunto;
            mensajeConAdjuntosDto.Destinatarios = destinatarios;
            mensajeConAdjuntosDto.Model = model;

            foreach (var adjunto in adjuntos)
            {
                mensajeConAdjuntosDto.AdjuntosNombres.Add(adjunto.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    adjunto.CopyTo(memoryStream);
                    mensajeConAdjuntosDto.Adjuntos.Add(memoryStream.ToArray());
                }
            }


            if (notificacionConfiguracion.ProcesarAsincronamente)
            {
                result = await ProcesamientoAsincronoConAdjuntosAsync(mensajeConAdjuntosDto);
            }
            else
            {
                result = await ProcesamientoDirectoConAdjuntosAsync(mensajeConAdjuntosDto);
            }

            return result;

        }

        private async Task<bool> ProcesamientoDirectoAsync(NotificacionMensajeDto datos)
        {
            logger.LogInformation("Procesamiento Directo.  Codigo Plantilla {plantillaCodigo}", datos.Codigo);
 
            var body = await templateRenderer.RenderAsync(
                        datos.Codigo.ToUpper(),
                        datos.Model
                        );
             
            logger.LogInformation("Enviar correo. Codigo Plantilla {plantillaCodigo}. CorreosDestinos: {correosDestinos}", datos.Codigo, datos.Destinatarios);


            await emailSender.SendAsync(
                 to: datos.Destinatarios,
                 subject: datos.Asunto,
                 body: body,
                 isBodyHtml: true
            );

            return true;
        }

        private async Task<bool> ProcesamientoAsincronoAsync(NotificacionMensajeDto notificationMessage)
        {
            logger.LogInformation("Procesamiento Asincrono.  Codigo Plantilla {plantillaCodigo}", notificationMessage.Codigo);

            var evento = ObjectMapper.Map<NotificacionMensajeDto, NotificacionMensajeEto>(notificationMessage);


            logger.LogInformation("Publicar evento de Enviar notificacion al correo. Codigo Plantilla {plantillaCodigo}.", notificationMessage.Codigo);

            await distributedEventBus.PublishAsync(evento);

            return true;
        }


        private async Task<bool> ProcesamientoDirectoConAdjuntosAsync(NotificacionMensajeConAdjuntosDto datos) {

            logger.LogInformation("Procesamiento Directo Con Adjuntos.  Codigo Plantilla {plantillaCodigo}", datos.Codigo);

            var body = await templateRenderer.RenderAsync(
                     datos.Codigo,
                     datos.Model
                     );

            var mailMessage = new MailMessage(await configuracionEnvioEmail.GetDefaultFromAddressAsync(),
                  datos.Destinatarios);

            mailMessage.Subject = datos.Asunto;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            int index = 0;
            foreach (var adjunto in datos.Adjuntos)
            {
                var memoryStream = new MemoryStream(adjunto);
                mailMessage.Attachments.Add(new Attachment(memoryStream, datos.AdjuntosNombres[index]));
                index++;
            }

            logger.LogInformation("Enviar correo Con Adjuntos. Codigo Plantilla {plantillaCodigo}. CorreosDestinos: {correosDestinos}", datos.Codigo, datos.Destinatarios);


            await emailSender.SendAsync(mailMessage);

            return true;
        }


        private async Task<bool> ProcesamientoAsincronoConAdjuntosAsync(NotificacionMensajeConAdjuntosDto datos)
        {

            logger.LogInformation("Procesamiento Asincrono Con Adjuntos.  Codigo Plantilla {plantillaCodigo}", datos.Codigo);

            var evento = ObjectMapper.Map<NotificacionMensajeConAdjuntosDto, NotificacionMensajeConAdjuntosEto>(datos);


            foreach (var adjunto in datos.Adjuntos)
            {
                evento.AdjuntosBase64.Add(Convert.ToBase64String(adjunto));
            }

            logger.LogInformation("Publicar evento de Enviar notificacion al correo. Codigo Plantilla {plantillaCodigo}.", datos.Codigo);

            await distributedEventBus.PublishAsync(evento);

            return true;
        }
    }


}
