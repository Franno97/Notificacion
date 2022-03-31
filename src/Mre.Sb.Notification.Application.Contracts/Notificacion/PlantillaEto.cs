using Volo.Abp.Application.Dtos;

namespace Mre.Sb.Notificacion
{
    public class PlantillaEto : IEntityDto<string>
    {
        public virtual string Id { get; set; }

        public virtual string Descripcion { get; set; } 

    }

}
