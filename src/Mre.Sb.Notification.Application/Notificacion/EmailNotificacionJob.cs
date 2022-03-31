using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace Mre.Sb.Notificacion.Application
{
    public class EmailNotificacionJob
       : AsyncBackgroundJob<EmailNotificacionArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public EmailNotificacionJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public override async Task ExecuteAsync(EmailNotificacionArgs args)
        {
            await _emailSender.SendAsync(
                args.Email,
                args.Titulo,
                args.Contenido
            );
        }
    }


    public class EmailNotificacionArgs
    {
        public string Email { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
    }

}
