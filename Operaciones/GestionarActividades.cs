using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WSArchivosLCH.Abstractions;
using WSArchivosLCH.Models;
using WSArchivosLCH.Services;

namespace WSArchivosLCH.Operaciones
{
    public class GestionarActividades : INotificarActividades
    {
        private readonly ILeschacoServices _leschacoServices;
        public GestionarActividades(ILeschacoServices leschacoServices) { 
            _leschacoServices = leschacoServices;
        }
        public async Task NotificarArribo()
        {
            try
            {
                var result = await _leschacoServices.ConsultaArribo();
                if (result != null)
                {
                    var actividades = JsonSerializer.Deserialize<List<VNotificacionArribo>>(result);
                    var envio = await _leschacoServices.EnviarArribo(actividades);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
