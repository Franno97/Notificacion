using AutoMapper;

namespace Mre.Sb.Notificacion
{
    public class NotificationApplicationAutoMapperProfile : Profile
    {
        public NotificationApplicationAutoMapperProfile()
        {
            
            CreateMap<Plantilla, PlantillaDto>();
            CreateMap<CrearActualizarPlantillaDto, Plantilla>();

            CreateMap<Plantilla, PlantillaEto>();

            CreateMap<NotificacionMensajeDto, NotificacionMensajeEto>();
            CreateMap<NotificacionMensajeConAdjuntosDto, NotificacionMensajeConAdjuntosEto>()
                .ForMember(x => x.AdjuntosBase64, map => map.Ignore())
                ;
             
        }
    }
}