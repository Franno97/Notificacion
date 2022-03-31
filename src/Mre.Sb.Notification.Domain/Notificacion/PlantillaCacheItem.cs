using System;

namespace Mre.Sb.Notificacion
{
    [Serializable]
    public class PlantillaCacheItem
    {
        public  string Id { get;  set; }

      
        public string Contenido { get; set; }

        public  string Descripcion { get; set; }

        private const string CacheKeyFormat = "tp:{0}";
         
        public static string CalcularClaveCache(string templateId)
        {
            return string.Format(CacheKeyFormat, templateId);
        }
    }
}
