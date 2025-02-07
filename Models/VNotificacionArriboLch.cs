using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Models
{
    public class VNotificacionArriboLch
    {
        public string datosPlanilla { get; set; } = null!;
        public string[] datosArchivos { get; set; } = null!;
        public string[] datosForm { get; set; } = null!;
        public string[] datosContactos { get; set; } = null!;
        public string[] idhouses { get; set; } = null!;
        public string idmaster { get; set; } = null!;
        public string modulo { get; set; } = null!;
        public string fchconfirmacion { get; set; }
        public string horaconfirmacion { get; set; }
        public string? registroadu { get; set; }
        public string fchregistroadu { get; set; }
        public string? bandera { get; set; }
        public string mnllegada { get; set; } = null!;
        public int? idmuelle { get; set; }
        public string combofactura { get; set; } = null!;
        public string mensaje { get; set; } = null!;
        public int? fchvaciado { get; set; }
        public int? horavaciado { get; set; }
        public string fchsyga { get; set; }
        public string module { get; set; } = null!;
        public string action { get; set; } = null!;
    }
}
