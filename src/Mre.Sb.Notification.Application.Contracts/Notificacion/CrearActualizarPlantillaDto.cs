using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Mre.Sb.Notificacion
{
    public class CrearActualizarPlantillaDto : IEntityDto<string>
    {
        [Required]
        [StringLength(PlantillaConsts.MaxIdLength)]
        public  string Id { get;  set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxDescripcionLength)]
        public virtual string Descripcion { get; set; }

       
        [Required]
        public string Contenido { get; set; }
    }
}
