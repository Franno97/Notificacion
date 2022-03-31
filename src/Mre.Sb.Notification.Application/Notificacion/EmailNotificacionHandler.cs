//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Emailing;
//using Volo.Abp.EventBus.Distributed;
//using Volo.Abp.Security.Encryption;
//using Volo.Abp.Uow;

//namespace Mre.Sb.Notificacion.Application
//{
//    public class EmailNotificacionHandler
//        : IDistributedEventHandler<EmailNotificacionEto>,
//          ITransientDependency
//    {
//        private readonly IEmailSender _emailSender;
//        private readonly ILogger<EmailNotificacionHandler> logger;

//        public EmailNotificacionHandler(IEmailSender emailSender,
//            ILogger<EmailNotificacionHandler> logger)
//        {
//            _emailSender = emailSender;
//            this.logger = logger;
//        }

//        [UnitOfWork]
//        public virtual async Task HandleEventAsync(EmailNotificacionEto eventData)
//        {
//            logger.LogDebug("Enviar Notificacion. Titulo {0} Email {1}, Contenido {2}",
//                eventData.Titulo,
//                eventData.Email,
//                eventData.Contenido
//                );
             

//            await _emailSender.SendAsync(
//                 to: eventData.Email,
//                 subject: eventData.Titulo,
//                 body: eventData.Contenido,
//                 isBodyHtml:eventData.EsHtml
//            );
//        }
//    }
//}
