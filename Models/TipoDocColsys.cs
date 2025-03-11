using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSArchivosLCH.Models
{
    public static class TipoDocColsys
    {
        private static Dictionary<string, string> tipos = new Dictionary<string, string>
        {
            { "1", "Master" },
            { "2", "Hbl" },
            { "3", "Tracking Naviera" },
            { "4", "Freight Manifiest" },
            { "5", "EIR o Notas de Inspeccion Contenedor" },
            { "6", "Pre-Factura" },
            { "7", "Factura a Clientes" },
            { "8", "Factura Nota Crédito Proveedores" },
            { "9", "Entrega de Antecedentes" },
            { "10", "Formulario 1166" },
            { "11", "Formulario DIAN 1207" },
            { "12", "Remisión de Documentos al Cliente" },
            { "16", "Registro Fotografico" },
            { "17", "Documentos liberados en Puerto" },
            { "18", "Contrato Comodato" },
            { "19", "Planilla de Recepción Deposito Usuario de Zona Franca" },
            { "20", "Reclamaciones-Requerimientos-PQR" },
            { "25", "Certificación de Fletes" },
            { "29", "Remisión de Documentos al Cliente" },
            { "45", "Migracion a siglo XXI" },
            { "83", "PQRS" },
            { "94", "Formato de Seguimiento" },
            { "116", "Formulario 1178" },
            { "117", "Novedades en Carga" }
        };     

        public static string ObtenerTipo(string IdTipo)
        {
            IdTipo = IdTipo.Trim() == "1" ? "10" : IdTipo; 
            return tipos[IdTipo];
        }
    }
}
