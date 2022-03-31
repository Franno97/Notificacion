using Grpc.Core;
using Mre.Sb.Notification.HttpApi;
using System.Threading.Tasks;
using static Mre.Sb.Notification.HttpApi.Notification;

namespace Mre.Sb.Notificacion.Grpc
{

    
    public class NotificationService : NotificationBase
    {
        public INotificadorAppService NotificationAppService { get; }


        public NotificationService(INotificadorAppService notificationAppService)
        {
            NotificationAppService = notificationAppService;
        }

      
        public override async Task<NotificationSendResponse> Send(NotificationSendRequest request, ServerCallContext context)
        {
            var notificationMessageDto = MapToDto(request);
            var result =  await NotificationAppService.EnviarAsync(notificationMessageDto);

            var notificationSendResponse = MapToResponse(result); 
            return notificationSendResponse;
        }


        private NotificacionMensajeDto MapToDto(NotificationSendRequest request)
        {
            var notificationMessageDto = new NotificacionMensajeDto();
            notificationMessageDto.Codigo = request.Code;
            notificationMessageDto.Asunto = request.Subject;
            notificationMessageDto.Destinatarios = request.Recipient;

            foreach (var item in request.Model)
            {
                notificationMessageDto.Model.Add(item.Key, item.Value);
            } 
            return notificationMessageDto;
        }

        private NotificationSendResponse MapToResponse(bool model)
        {
            var notificationSendResponse = new NotificationSendResponse();
            notificationSendResponse.Status = model;
            return notificationSendResponse;
        }
    }
}
