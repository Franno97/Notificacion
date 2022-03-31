using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mre.Sb.Notificacion
{
    public class NotificacionMensajeConAdjuntosEto
    {
        public NotificacionMensajeConAdjuntosEto()
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

        /// <summary>
        /// Listado nombres archivos adjuntos. 
        /// </summary>
        public IList<string> AdjuntosNombres { get; set; } = new List<string>();

        /// <summary>
        /// Listado archivos adjuntos. Strings formato Base64
        /// </summary>
        public IList<string> AdjuntosBase64 { get; set; } = new List<string>();
    }
}
