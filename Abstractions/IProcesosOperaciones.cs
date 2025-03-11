using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Abstractions
{
    public interface IProcesosOperaciones
    {
        public Task EnviarArchivosLch();
        public Task CopiarArchivosFile();
        public Task EnviarArchivosLchFotograficos();
    }
}
