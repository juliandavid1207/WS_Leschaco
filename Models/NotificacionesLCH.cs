using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Models
{
    public class NotificacionesLCH
    {
        public int IdNotificacion { get; set; }
        public decimal CodigoNotificacion { get; set; }
        public string NumeroNotificacion { get; set; } = null!;
        public DateTime FechaNotificacion { get; set; }
        public bool Notificado { get; set; }
    }
}
