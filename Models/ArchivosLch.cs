using System;
using System.Collections.Generic;

namespace WSArchivosLCH.Models
{
    public class ArchivosLch
    {
        public int IdRegistro { get; set; }
        public string? CodigoFile { get; set; }
        public string? CodigoArchivo { get; set; }
        public string? ArchivoBase64 { get; set; }
        public string? FilecabDocumen { get; set; }
        public string? FilecabNumero { get; set; }
        public string? BlHijo { get; set; }
        public bool? Copiado { get; set; }
    }
}
