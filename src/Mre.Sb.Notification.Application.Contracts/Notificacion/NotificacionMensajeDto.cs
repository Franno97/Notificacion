using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mre.Sb.Notificacion
{
    public class NotificacionMensajeDto {
        public NotificacionMensajeDto()
        {
            Model = new Dictionary<string, object>();
        }

        [Required]
        [StringLength(maximumLength: PlantillaConsts.MaxIdLength)]
        public string Codigo { get; set; }

        [Required]
        public string Asunto { get; set; }

        public string Destinatarios { get; set; }

  
        [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
        public Dictionary<string, object> Model { get; set; }
    }

}
