using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mre.Sb.Notificacion
{
    public class NotificacionMensajeConAdjuntosDto
    {
        

        [Required]
        [StringLength(maximumLength: PlantillaConsts.MaxIdLength)]
        public string Codigo { get; set; }

        [Required]
        public string Asunto { get; set; }

        public string Destinatarios { get; set; }


        [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
        public Dictionary<string, string> Model { get; set; } = new Dictionary<string, string>();


        /// <summary>
        /// Listado nombres archivos adjuntos. 
        /// </summary>
        public IList<string> AdjuntosNombres { get; set; } = new List<string>();

        /// <summary>
        ///  Listado archivos adjuntos. (array bytes)
        /// </summary>
        public IList<byte[]> Adjuntos { get; set; } = new List<byte[]>();

    }

}
