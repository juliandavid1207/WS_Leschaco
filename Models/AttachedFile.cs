using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Models
{
    public class AttachedFile
    {
        public string referencia {  get; set; }
        public string archivoBase64 { get; set; }
        public string tipoDocumento { get; set; }
        public string nombreArchivo { get; set; }
        public string hbl {  get; set; }
        public string fecha { get; set; }
        public string usuarioCreado { get; set; }
        public string mime { get; set; }
        public string idmaster { get; set; }
        public string idhouse { get; set; }
        public string planilla { get; set; }
     

    }
}
