//using Microsoft.Extensions.Logging;
//using System.IO;
//using System.Net.Mail;
//using System.Threading.Tasks;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Emailing;

//namespace Mre.Sb.Notificacion.Application
//{
//    public class EmailNotificacionConAdjuntoHander : IEmailNotificacionConAdjuntoHander, ITransientDependency
//    {
//        private readonly IEmailSender emailSender;
//        private readonly IEmailSenderConfiguration configuracionEnvioEmail;
//        private readonly ILogger<EmailNotificacionConAdjuntoHander> logger;

//        public EmailNotificacionConAdjuntoHander(IEmailSender emailSender,
//            IEmailSenderConfiguration configuracionEnvioEmail,
//            ILogger<EmailNotificacionConAdjuntoHander> logger)
//        {
//            this.emailSender = emailSender;
//            this.configuracionEnvioEmail = configuracionEnvioEmail;
//            this.logger = logger;
//        }

//        public async Task HandleEventAsync(EmailNotificacionConAdjunto datos)
//        {

//            var mailMessage = new MailMessage(await configuracionEnvioEmail.GetDefaultFromAddressAsync(),
//                datos.Email);

//            mailMessage.Subject = datos.Titulo;
//            mailMessage.Body = datos.Contenido;
//            mailMessage.IsBodyHtml = datos.EsHtml;

//            foreach (var adjunto in datos.Adjuntos)
//            {
//                var memoryStream = new MemoryStream();
//                adjunto.CopyTo(memoryStream);
//                mailMessage.Attachments.Add(new Attachment(memoryStream, adjunto.FileName));

//            }

//            await emailSender.SendAsync(mailMessage);


//        }
//    }


//}
