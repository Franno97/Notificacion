using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Mre.Sb.Notificacion
{



    public class Plantilla : Entity<string>
    {
        [Required]
        [StringLength(PlantillaConsts.MaxIdLength)]
        public override string Id { get; protected set; }

        [Required]
        [StringLength(DomainCommonConsts.MaxDescripcionLength)]
        public virtual string Descripcion { get; set; }

         
        [Required]
        public string Contenido { get; set; }

        protected Plantilla()
        {
        }

        public Plantilla(string id)
         : base(id.ToUpper())
        {

        }
    }

}
