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
                    if (facturas != null)  
                    {
                        foreach (var fac in facturas)
                        {
                            if (fac.archivos != null && !string.IsNullOrEmpty(fac.archivos.RutaArchivo)) 
                            {
                                //fac.archivos.Base64 = await _leschacoServices.EncodeFile(fac.archivos.RutaArchivo);
                            }
                        }

                        var envio = await _leschacoServices.EnviarArchivosLch(facturas);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task EnviarArchivosLchFotograficos()
        {
            try
            {
                var result = await _leschacoServices.ConsultarArchivosFileFotograficos();
                if (result != null)
                {
                    var facturas = JsonSerializer.Deserialize<GetArchivosFile>(result);
                    if (facturas != null)
                    {
                        var ruta = facturas.archivos.RutaArchivo;
                        facturas.archivos.Base64 = await _leschacoServices.EncodeFile(ruta);
                        var envio = await _leschacoServices.EnviarArchivosLchFotograficos(facturas);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
