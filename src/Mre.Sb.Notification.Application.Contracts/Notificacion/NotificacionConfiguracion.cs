using System;
using System.Collections.Generic;
using System.Text;

namespace Mre.Sb.Notificacion
{
    public class NotificacionConfiguracion
    {
        public string EnviarEmail { get; set; }

        /// <summary>
        /// Al colocar una lista  destinatarios "Emails- separados con coma", todas las notificaciones seran enviadas a estos correos. 
        /// Esto permite ignorar los destinatarios enviados en las notificaciones
        /// </summary>
        public string DestinatariosReemplazar { get; set; }

        /// <summary>
        /// El proceso notificaciones se puede realizar asincronamente utilizando eventos
        /// </summary>
        public bool ProcesarAsincronamente { get; set; } = true;

    }
}
