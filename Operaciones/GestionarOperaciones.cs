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
    public class GestionarOperaciones : IProcesosOperaciones
    {
        private ILeschacoServices _leschacoServices;
        public GestionarOperaciones(ILeschacoServices leschacoServices)
        {
            _leschacoServices = leschacoServices;
        }

        public async Task CopiarArchivosFile()
        {
            try
            {
                var result = await _leschacoServices.ConsultaArchivosXCopiar();
                if (result != null)
                {
                    var archivos = JsonSerializer.Deserialize<List<ArchivosLch>>(result);
                    var copia = await _leschacoServices.CopiarArchivos(archivos);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task EnviarArchivosLch()
        {
            try
            {
                var result = await _leschacoServices.ConsultarArchivosFile();
                if (result != null)
                {
                    var facturas = JsonSerializer.Deserialize<List<GetArchivosFile>>(result);
                    var envio = await _leschacoServices.EnviarArchivosLch(facturas);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
