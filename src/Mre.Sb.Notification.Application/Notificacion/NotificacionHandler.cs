using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Mre.Sb.Notificacion.Localization;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.TextTemplating;
using Volo.Abp.Uow;

namespace Mre.Sb.Notificacion.Application
{
    public class NotificacionHandler
        : IDistributedEventHandler<NotificacionMensajeEto>,
          ITransientDependency
    {
        private readonly IEmailSender emailSender;
        private readonly ITemplateRenderer templateRenderer;
        private readonly ITemplateDefinitionManager templateDefinitionManager;
        private readonly IStringLocalizer<NotificationResource> localizer;
        private readonly ILogger<NotificacionHandler> logger;

        public NotificacionHandler(IEmailSender emailSender,
            ITemplateRenderer templateRenderer,
            ITemplateDefinitionManager templateDefinitionManager,
            IStringLocalizer<NotificationResource> localizer,
            ILogger<NotificacionHandler> logger)
        {
            this.emailSender = emailSender;
            this.templateRenderer = templateRenderer;
            this.templateDefinitionManager = templateDefinitionManager;
            this.localizer = localizer;
            this.logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(NotificacionMensajeEto eventData)
        {
            logger.LogDebug("Receptar evento Notificacion. Plantilla {0} Destinatarios {1}",
               eventData.Codigo.ToUpper(),
               eventData.Destinatarios
               );

            var templateDefinition = templateDefinitionManager.GetOrNull(eventData.Codigo.ToUpper());

            if (templateDefinition == null)
            {
                var msg = string.Format(localizer["Plantilla:NoExiste"], eventData.Codigo.ToUpper());
                throw new AbpException(msg);
            }

            var body = await templateRenderer.RenderAsync(
                        eventData.Codigo.ToUpper(),
                        eventData.Model
                        );
              
            logger.LogInformation("Enviar correo. Codigo Plantilla {plantillaCodigo}. CorreosDestinos: {correosDestinos}", eventData.Codigo, eventData.Destinatarios);


            await emailSender.SendAsync(
                 to: eventData.Destinatarios,
                 subject: eventData.Asunto,
                 body: body,
                 isBodyHtml: true
            );
        }
    }


    public class NotificacionConAdjuntosHandler
        : IDistributedEventHandler<NotificacionMensajeConAdjuntosEto>,
          ITransientDependency
    {
        private readonly IEmailSender emailSender;
        private readonly IEmailSenderConfiguration configuracionEnvioEmail;
        private readonly ITemplateRenderer templateRenderer;
        private readonly ITemplateDefinitionManager templateDefinitionManager;
        private readonly IStringLocalizer<NotificationResource> localizer;
        private readonly ILogger<NotificacionConAdjuntosHandler> logger;

        public NotificacionConAdjuntosHandler(IEmailSender emailSender,
            IEmailSenderConfiguration configuracionEnvioEmail,
            ITemplateRenderer templateRenderer,
            ITemplateDefinitionManager templateDefinitionManager,
            IStringLocalizer<NotificationResource> localizer,
            ILogger<NotificacionConAdjuntosHandler> logger)
        {
            this.emailSender = emailSender;
            this.configuracionEnvioEmail = configuracionEnvioEmail;
            this.templateRenderer = templateRenderer;
            this.templateDefinitionManager = templateDefinitionManager;
            this.localizer = localizer;
            this.logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(NotificacionMensajeConAdjuntosEto eventData)
        {
            logger.LogDebug("Receptar evento Notificacion con Adjuntos . Plantilla {plantillaCodigo} Destinatarios {1}",
               eventData.Codigo.ToUpper(),
               eventData.Destinatarios
               );

            var templateDefinition = templateDefinitionManager.GetOrNull(eventData.Codigo.ToUpper());

            if (templateDefinition == null)
            {
                var msg = string.Format(localizer["Plantilla:NoExiste"], eventData.Codigo.ToUpper());
                throw new AbpException(msg);
            }

            var body = await templateRenderer.RenderAsync(
                        eventData.Codigo.ToUpper(),
                        eventData.Model
                        );


            var mailMessage = new MailMessage(await configuracionEnvioEmail.GetDefaultFromAddressAsync(),
                eventData.Destinatarios);

            mailMessage.Subject = eventData.Asunto;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            int index = 0;
            foreach (var adjunto in eventData.AdjuntosBase64)
            {
                var memoryStream = new MemoryStream(Convert.FromBase64String(adjunto));
                mailMessage.Attachments.Add(new Attachment(memoryStream, eventData.AdjuntosNombres[index]));
                index++;

            }

            logger.LogInformation("Enviar correo. Codigo Plantilla {plantillaCodigo}. CorreosDestinos: {correosDestinos}", eventData.Codigo, eventData.Destinatarios);

            await emailSender.SendAsync(mailMessage);

        }
    }
}
