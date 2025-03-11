using System;
using System.Collections.Generic;

namespace WSArchivosLCH.Models
{
    public partial class VNotificacionAtraso
    {
        public string DatosPlanilla { get; set; } = null!;
        public string Idhouses { get; set; } = null!;
        public string Idmaster { get; set; } = null!;
        public string Modulo { get; set; } = null!;
        public DateTime Fchconfirmacion { get; set; }
        public string? Horaconfirmacion { get; set; }
        public string? Registroadu { get; set; }
        public DateTime? Fchregistroadu { get; set; }
        public string? Bandera { get; set; }
        public string Mnllegada { get; set; } = null!;
        public string? Idmuelle { get; set; }
        public string Combofactura { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public int? Fchvaciado { get; set; }
        public int? Horavaciado { get; set; }
        public int? Fchsyga { get; set; }
        public string Status { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Hbl { get; set; } = null!;
        public string Referencia { get; set; } = null!;
    }
}
