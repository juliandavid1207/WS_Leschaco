using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Models
{
    public class GetArchivosFile
    {
        public ArchivosFile archivos { get; set; }

        public bool ClienteExterno { get; set; }
    }
}
