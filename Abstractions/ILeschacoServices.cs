using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSArchivosLCH.Models;

namespace WSArchivosLCH.Abstractions
{
    public interface ILeschacoServices
    {
        public Task<string> ConsultarArchivosFile();
        public Task<string> EnviarArchivosLch(List<GetArchivosFile> archivos);
        public Task<string> ConsultaArribo();
        public Task<string> ConsultaDesconsolidacion();
        public Task<string> ActualizarNotificados(NotificacionesLCH notificacion);
        public Task<string> EnviarArribo(List<VNotificacionArribo> lstArchivos);
        public Task<string> EnviarDesconsolidacion(List<VNotificacionDesconsolidacion> lstArchivos);
        public Task<string> ConsultaArchivosXCopiar();
        public Task<string> CopiarArchivos(List<ArchivosLch> archivosXCopiar);
        public Task<string> EnviarArchivosLchFotograficos(GetArchivosFile archivos);
        public Task<string> ConsultarArchivosFileFotograficos();
        public Task<string> EncodeFile(string path);
        public Task<string> EnviarAtraso(List<VNotificacionAtraso> lstArchivos);
        public Task<string> ConsultaAtraso();
    }
}
