using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Abstractions
{
    public interface INotificarActividades
    {
        public Task NotificarArribo();
        public Task NotificarDesconsolidacion();
        public Task NotificarAtraso();
    }
}
