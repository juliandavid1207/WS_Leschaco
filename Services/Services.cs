using ApiComodato.Models.Lch_Agencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using WSArchivosLCH.Abstractions;
using WSArchivosLCH.Models;

namespace WSArchivosLCH.Services
{
    public class Services : ILeschacoServices
    {
        private readonly IHttpClientFactory _httpClient;
    
        public Services(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;        
        }
        public async Task<string> ConsultarArchivosFile()
        {
            try
            {
                var client = _httpClient.CreateClient("OwnData");
                var response = await client.GetAsync("GetArchivosFile1X3");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;                  

            }
            catch (Exception ex) 
            {
                return null;
            }

        }

        public async Task<string> ConsultarArchivosFileFotograficos()
        {
            try
            {
                var client = _httpClient.CreateClient("OwnData");
                var response = await client.GetAsync("GetArchivosLchFotograficos1X3");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<string> EncodeFile(string path)
        {
            var fileEncode= Convert.ToBase64String(File.ReadAllBytes(path));
            return fileEncode;
        }

        public async Task<string> ConsultaArribo()
        {
            try
            {
                var client = _httpClient.CreateClient("OwnData");
                var response = await client.GetAsync("GetPendientesArribo1X3");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public async Task<string> ActualizarArchivoFile(ArchivosEnviados archivo)
        {
            try
            {
                if (archivo.Copiado==true)
                {
                    var client = _httpClient.CreateClient("OwnData");
                    StringContent content = new StringContent(JsonSerializer.Serialize(archivo), Encoding.UTF8, "application/json");
                    var response = await client.PatchAsync("MarcarCopiados1X3", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        } 

        public async Task<string> EnviarArchivosLch(List<GetArchivosFile> lstArchivos)
        {
            try
            {
                foreach (var archivo in lstArchivos)
                {
                    var json = await ObtenerJsonAttachedFile(archivo.archivos);
                    var xml = ObtenerXmlArchivos(json, "9");
                    var result = await EnviarXml(xml, archivo.archivos.IdArchivo);     
                    if (result.Copiado==true)
                        await ActualizarArchivoFile(result);
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string ObtenerXml(string json, string tipo)
        {

            string xml = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"https://colsys.com.co/ws/fam/IntegracionFamWS\">\r\n   " +
                        $"<soapenv:Header/>\r\n   <soapenv:Body>\r\n      <ws:tipoSolicitud>\r\n         <companyIdSap>1</companyIdSap>\r\n         " +
                        $"<nit>20058</nit>\r\n         <tipo>{tipo}</tipo>\r\n         <json>{json}</json>\r\n      </ws:tipoSolicitud>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";
            return xml;

            //string xml = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://10.192.5.28/ws/fam/IntegracionFamWS\">\r\n   " +
            //            $"<soapenv:Header/>\r\n   <soapenv:Body>\r\n      <ws:tipoSolicitud>\r\n         <companyIdSap>1</companyIdSap>\r\n         " +
            //            $"<nit>9011</nit>\r\n         <tipo>{tipo}</tipo>\r\n         <json>{json}</json>\r\n      </ws:tipoSolicitud>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";


            //return xml;

        }

        private string ObtenerXmlArchivos(string json, string tipo)
        {

            string xml = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"https://colsys.com.co/ws/fam/IntegracionFamWS\">\r\n   " +
                        $"<soapenv:Header/>\r\n   <soapenv:Body>\r\n      <ws:tipoSolicitud>\r\n         <companyIdSap>1</companyIdSap>\r\n         " +
                        $"<nit>20058</nit>\r\n         <tipo>{tipo}</tipo>\r\n         <json>[{json}]</json>\r\n      </ws:tipoSolicitud>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";


            //string xml = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ws=\"http://10.192.5.28/ws/fam/IntegracionFamWS\">\r\n   " +
            //            $"<soapenv:Header/>\r\n   <soapenv:Body>\r\n      <ws:tipoSolicitud>\r\n         <companyIdSap>1</companyIdSap>\r\n         " +
            //            $"<nit>9011</nit>\r\n         <tipo>{tipo}</tipo>\r\n         <json>[{json}]</json>\r\n      </ws:tipoSolicitud>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";


            return xml;

        }

        public async Task<string> ObtenerJsonAttachedFile(ArchivosFile archivo)
        {
            var adicionales = await ObtenerAdicionales(archivo.File, archivo.Hbl);
            
            if (adicionales == null)
            {
                adicionales = new AdicionalesArchivosLch
                {
                    IdMaster = "",
                    IdHouse=""
                    
                };
            }
                

            var attached = new AttachedFile
            {
                referencia = archivo.File.Trim(),
                archivoBase64 = archivo.Base64.Trim(),
                tipoDocumento = archivo.Codigo.Trim() == "1" ? "10" : archivo.Codigo.Trim(),
                nombreArchivo = TipoDocColsys.ObtenerTipo(archivo.Codigo.Trim()) + " " + archivo.Hbl.Trim()+archivo.Extension.Trim(),
                fecha = Convert.ToDateTime(archivo.FechaCargue).ToString("yyyy-MM-dd"),
                usuarioCreado = "fam",
                hbl = archivo.Hbl.Trim(),
                mime= archivo.Codigo == "16" ? "application/zip" : "application/pdf",
                idmaster = adicionales.IdMaster,
                idhouse = adicionales.IdHouse,
                planilla = archivo.NombreCopia == null ? "" : archivo.NombreCopia.Trim()
            };
            var json = JsonSerializer.Serialize(attached);             
            
            return json;
        }

        private async Task<AdicionalesArchivosLch> ObtenerAdicionales(string codFile, string hbl)
        {
            try
            {

                var client = _httpClient.CreateClient("OwnData");
                var parametro1 = codFile;
                var parametro2 = hbl;

                var url = $"GetAdicionalesArchivos1X3?codFile={parametro1}&Hbl={parametro2}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var adicionales = await response.Content.ReadAsStringAsync();
                    var adicionales1 = JsonSerializer.Deserialize<AdicionalesArchivosLch>(adicionales);
                    return adicionales1;
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public string[] ObtenerJsonArribo(VNotificacionArribo archivo)
        {
            string[] datos;
            var arribo = new VNotificacionArriboLch
            {
                datosPlanilla =archivo.DatosPlanilla.Trim(),
                datosArchivos = new string[] { },
                datosForm = new string[] { },
                datosContactos = new string[] { },
                idhouses = new string[] { archivo.Idhouses.Trim() },
                idmaster = archivo.Idmaster.Trim(),
                modulo = archivo.Modulo.Trim(),
                fchconfirmacion = Convert.ToDateTime(archivo.Fchconfirmacion).ToString("yyyy-MM-dd"),
                horaconfirmacion = archivo.Horaconfirmacion.Trim() != null ? archivo.Horaconfirmacion.Trim() : "00:00:00",
                registroadu = archivo.Registroadu.Trim(),
                fchregistroadu = Convert.ToDateTime(archivo.Fchregistroadu).ToString("yyyy-MM-dd"),
                bandera = archivo.Bandera.Trim() != null ? archivo.Bandera.Trim() : "",
                mnllegada = archivo.Mnllegada.Trim(),
                idmuelle = Convert.ToInt32(archivo.Idmuelle),
                combofactura = archivo.Combofactura.Trim(),
                mensaje = archivo.Mensaje.Trim(),
                fchvaciado = null,
                horavaciado = null,
                fchsyga = null,
                module = "status",
                action = "crearStatus",
                hbl = archivo.Hbl.Trim(),
                usucreado = "fam",
                tipo = "confirmacion_llegada"
            };
            var json = JsonSerializer.Serialize(arribo);
            datos =new string[] {json,archivo.Numero};
            return datos;
        }

        public string[] ObtenerJsonDesconsolidacion(VNotificacionDesconsolidacion archivo)
        {
            string[] datos;
            var desconsolidacion = new VNotificacionDesconsolidacionLch
            {
                datosPlanilla = archivo.DatosPlanilla.Trim(),
                datosArchivos = new string[] { },
                datosForm = new DatosFormDesconsolidacion[] {
                    new DatosFormDesconsolidacion
                    {
                        idhouse = archivo.Idhouses.Trim(),
                        mensaje = archivo.Mensaje.Trim(),
                        fchstatus = Convert.ToDateTime(archivo.Fchconfirmacion).ToString("yyyy-MM-dd"),
                        horastatus = archivo.Horaconfirmacion.Trim() != null ? archivo.Horaconfirmacion.Trim() : "00:00:00",
                    }
                },
                datosContactos = new string[] { },
                idhouses = new string[] { archivo.Idhouses.Trim() },
                idmaster = archivo.Idmaster.Trim(),
                hbl = archivo.Hbl.Trim(),
                modulo = "desconsolidacion",
                fchconfirmacion = "",               
                registroadu = "",
                fchregistroadu = "",
                bandera = "",
                mnllegada = "",
                idmuelle = "",
                combofactura = "ffletes",
                mensaje = "",
                fcharribo = "",
                fchvaciado = "",
                fchsyga = "",
                module = "status",
                action = "crearStatus",
                tipo = "desconsolidacion",
                usucreado = "fam"               
            };
            var json = JsonSerializer.Serialize(desconsolidacion);
            datos = new string[] { json, archivo.Numero };
            return datos;
        }

        public string[] ObtenerJsonAtraso(VNotificacionAtraso archivo)
        {
            string[] datos;
            var desconsolidacion = new VNotificacionAtrasoLch
            {
                datosPlanilla = archivo.DatosPlanilla.Trim(),
                datosArchivos = new string[] { },
                datosForm = new DatosFormAtraso[] {
                    new DatosFormAtraso
                    {
                        idhouse = archivo.Idhouses.Trim(),
                        mensaje = "Notificación de atraso de mercancía.",
                        fchstatus = DateTime.Now.ToString("yyyy-MM-dd"),
                        horastatus = DateTime.Now.ToString("HH:mm:ss") 
                    }
                },
                datosContactos = new string[] { },
                idhouses = new string[] { archivo.Idhouses.Trim() },
                idmaster = archivo.Idmaster.Trim(),
                hbl = archivo.Hbl.Trim(),
                modulo = "status",
                fchconfirmacion = "",
                registroadu = "",
                fchregistroadu = "",
                bandera = "",
                mnllegada = "",
                idmuelle = "",
                combofactura = "ffletes",
                mensaje = "",
                fcharribo = Convert.ToDateTime(archivo.Fchconfirmacion).ToString("yyyy-MM-dd"),
                fchvaciado = "",
                fchsyga = "",
                module = "status",
                action = "crearStatus",
                tipo = "novedades_atraso",
                usucreado = "fam"
            };
            var json = JsonSerializer.Serialize(desconsolidacion);
            datos = new string[] { json, archivo.Numero };
            return datos;
        }

        public async Task<ArchivosEnviados> EnviarXml(string xml, int id)
        {
            ArchivosEnviados archivoEnviado = new ArchivosEnviados
            {
                IdArchivo = id,
                Copiado = false,
                Mensaje = ""
            };
            try
            {
                var client = _httpClient.CreateClient("ApiLeschaco");
                StringContent content = new StringContent(xml, Encoding.UTF8, "text/xml");
                var response = await client.PostAsync("", content);  

                if (response.IsSuccessStatusCode)
                {
                    string responseXml = await response.Content.ReadAsStringAsync();

                    if(responseXml.Contains("VERIFICAR GUARDADO DE DATOS E EMAILS"))
                    {
                        archivoEnviado.Copiado = true;
                        archivoEnviado.Mensaje = responseXml;
                        return archivoEnviado;
                    }

                    if (responseXml.Contains("INFORMACION DE CONFIRMACION DE LLEGADA RECIBIDA"))
                    {
                        archivoEnviado.Copiado = true;
                        archivoEnviado.Mensaje = responseXml;
                        return archivoEnviado;
                    }

                    if (responseXml.Contains("El Archivo se subio de manera exitosa a la referencia"))
                    {
                        archivoEnviado.Copiado = true;
                        archivoEnviado.Mensaje = responseXml;
                        return archivoEnviado;
                    }                    

                    if (responseXml.Contains("Las comunicaciones se han enviado correctamente! llegada"))
                    {
                        archivoEnviado.Copiado = true;
                        archivoEnviado.Mensaje = responseXml;
                        return archivoEnviado;
                    }

                    if (responseXml.Contains("Las comunicaciones se han enviado correctamente! desconsolidacion"))
                    {
                        archivoEnviado.Copiado = true;
                        archivoEnviado.Mensaje = responseXml;
                        return archivoEnviado;
                    }
                    if (responseXml.Contains(" Las comunicaciones se han enviado correctamente! status"))
                    {
                        archivoEnviado.Copiado = true;
                        archivoEnviado.Mensaje = responseXml;
                        return archivoEnviado;
                    }
                   



                    XDocument xmlDoc = XDocument.Parse(responseXml);

                    var jsonNode = xmlDoc.Descendants("item")
                                     .Where(x => (string)x.Element("key") == "mensaje:")
                                     .FirstOrDefault();

                    if (jsonNode != null)
                    {
                        var jsonString = jsonNode.Element("value")?.Value;

                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            if (jsonString.Contains("El Archivo se subio de manera exitosa a la referencia"))
                            {
                                archivoEnviado.Copiado = true;
                                archivoEnviado.Mensaje = jsonString;
                                return archivoEnviado;
                            }
                         
                            archivoEnviado.Mensaje = jsonString;
                            return archivoEnviado;

                        }
                        else
                        {
                            archivoEnviado.Mensaje = "No se obtuvo el mensaje";
                            return archivoEnviado;
                        }
                    }
                    archivoEnviado.Mensaje = "No se obtuvo el mensaje";
                    return archivoEnviado;                  
                }

                archivoEnviado.Mensaje = await response.Content.ReadAsStringAsync();
                return archivoEnviado;
            }
            catch (Exception ex)
            {
                archivoEnviado.Mensaje = $"Error en el WS:{ex.Message}";
                return archivoEnviado;
            }
        }

        public async Task<string> ActualizarNotificados(NotificacionesLCH notificacion)
        {
            try
            {               
                var client = _httpClient.CreateClient("OwnData");
                StringContent content = new StringContent(JsonSerializer.Serialize(notificacion), Encoding.UTF8, "application/json");
                var response = await client.PatchAsync("MarcarNotificados1X3", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> ActualizarCopiados(ArchivosLch archivo)
        {
            try
            {
                var client = _httpClient.CreateClient("OwnData");
                StringContent content = new StringContent(JsonSerializer.Serialize(archivo), Encoding.UTF8, "application/json");
                var response = await client.PatchAsync("MarcarCopiadosOperacion1X3", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<string> EnviarArribo(List<VNotificacionArribo> lstArchivos)
        {
            try
            {
                foreach (var archivo in lstArchivos)
                {
                    var json = ObtenerJsonArribo(archivo);
                    var xml = ObtenerXml(json[0], "12");
                    var result = await EnviarXml(xml, Convert.ToInt32(json[1].Trim()));
                    if (result.Copiado == true)
                        await ActualizarNotificados(new NotificacionesLCH { CodigoNotificacion = 48, NumeroNotificacion = json[1],Estimada=0 });
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> EnviarDesconsolidacion(List<VNotificacionDesconsolidacion> lstArchivos)
        {
            try
            {
                foreach (var archivo in lstArchivos)
                {
                    var json = ObtenerJsonDesconsolidacion(archivo);
                    var xml = ObtenerXml(json[0], "12");
                    var result = await EnviarXml(xml, Convert.ToInt32(json[1].Trim()));
                    if (result.Copiado == true)
                        await ActualizarNotificados(new NotificacionesLCH { CodigoNotificacion = 66, NumeroNotificacion = json[1], Estimada=0 });
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> ConsultaArchivosXCopiar()
        {
            try
            {
                var client = _httpClient.CreateClient("OwnData");
                var response = await client.GetAsync("GetArchivosLch1X3");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }



        public async Task<string> CopiarArchivos(List<ArchivosLch> archivosXCopiar)
        {
            try
            {
                foreach(var archivo in archivosXCopiar)
                {
                    string outputFolderPath = $"G:\\WEBGOL\\GOL-ACI\\DATOSLCH\\ArchivosFile\\{archivo.CodigoFile}\\{archivo.BlHijo}\\";
                    //string outputFolderPath = $"C:\\Users\\jotalora\\OneDrive - FAM Team\\Desktop\\PruebaCopia\\{archivo.CodigoFile}\\{archivo.BlHijo}\\";
                    string tipoArchivo = archivo.CodigoArchivo == "100" ? "MBL" : "HBL";
                    string outputFileName = $"{archivo.CodigoArchivo} {tipoArchivo} {DateTime.Now.ToString("dd-MM-yy HHmmssfff")}.pdf";                   
                    byte[] fileBytes = Convert.FromBase64String(archivo.ArchivoBase64);
               
                    if (!Directory.Exists(outputFolderPath))
                    {
                        Directory.CreateDirectory(outputFolderPath);
                    }
                   
                    string outputFilePath = Path.Combine(outputFolderPath, outputFileName);
                    File.WriteAllBytes(outputFilePath, fileBytes);
                    await ActualizarCopiados(archivo);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> EnviarArchivosLchFotograficos(GetArchivosFile lstArchivos)
        {
            try
            {
                var json = await ObtenerJsonAttachedFile(lstArchivos.archivos);
                var xml = ObtenerXmlArchivos(json, "9");
                var result = await EnviarXml(xml, lstArchivos.archivos.IdArchivo);
                if(result.Copiado==true)
                    await ActualizarArchivoFile(result);

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> ConsultaDesconsolidacion()
        {
            try
            {
                var client = _httpClient.CreateClient("OwnData");
                var response = await client.GetAsync("GetPendientesDesconsolidacion1X3");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> EnviarAtraso(List<VNotificacionAtraso> lstArchivos)
        {
            try
            {
                foreach (var archivo in lstArchivos)
                {
                    var json = ObtenerJsonAtraso(archivo);
                    var xml = ObtenerXml(json[0], "12");
                    var result = await EnviarXml(xml, Convert.ToInt32(json[1].Trim()));
                    if (result.Copiado == true)
                        await ActualizarNotificados(new NotificacionesLCH { CodigoNotificacion = 48, NumeroNotificacion = json[1],Estimada=1 });
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }    

        public async Task<string> ConsultaAtraso()
        {
            try
            {
                var client = _httpClient.CreateClient("OwnData");
                var response = await client.GetAsync("GetPendientesAtraso1X3");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
