namespace Mre.Sb.Notificacion
{
    public static class NotificationDbProperties
    {
        public static string DbTablePrefix { get; set; } = "";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Notificacion";
 
  
    }

    public static class NotificationConstants
    {
       

        public const string CarpetaPlantillas = "/Plantillas/Scriban";



    }
}
