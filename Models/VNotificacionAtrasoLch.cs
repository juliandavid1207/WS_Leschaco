using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Models
{
    public class VNotificacionAtrasoLch
    {
        public string datosPlanilla { get; set; } = null!;
        public string[] datosArchivos { get; set; } = null!;
        public DatosFormAtraso[] datosForm { get; set; } = null!;
        public string[] datosContactos { get; set; } = null!;
        public string[] idhouses { get; set; } = null!;
        public string idmaster { get; set; } = null!;
        public string hbl { get; set; }
        public string modulo { get; set; } = null!;
        public string fchconfirmacion { get; set; }
        public string? registroadu { get; set; }
        public string fchregistroadu { get; set; }
        public string? bandera { get; set; }
        public string mnllegada { get; set; } = null!;
        public string? idmuelle { get; set; }
        public string combofactura { get; set; } = null!;
        public string mensaje { get; set; } = null!;
        public string? fcharribo { get; set; }
        public string? fchvaciado { get; set; }   
        public string fchsyga { get; set; }
        public string module { get; set; } = null!;
        public string action { get; set; } = null!;
        public string? tipo { get; set; }
        public string? usucreado { get; set; }
      
    }
}
