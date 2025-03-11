using System;
using System.Collections.Generic;

namespace WSArchivosLCH.Models
{
    public partial class ArchivosFile
    {
        public int IdArchivo { get; set; }
        public string? RutaArchivo { get; set; }
        public string? NombreArchivo { get; set; }
        public string? File { get; set; }
        public string? Codigo { get; set; }
        public string? NombreCopia { get; set; }
        public string? Hbl { get; set; }
        public string? Extension { get; set; }
        public DateTime? FechaCargue { get; set; }
        public DateTime? FechaCopia { get; set; }
        public bool? Copiado { get; set; }
        public string? Base64 { get; set; }
        public string? MensajeLch { get; set; }
    }
}
