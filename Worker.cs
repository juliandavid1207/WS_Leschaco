using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using WSArchivosLCH.Abstractions;
using WSArchivosLCH.Models;

namespace WSArchivosLCH
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILeschacoServices _services;
        private readonly INotificarActividades _actividades;
        private readonly IProcesosOperaciones _operaciones;

        public Worker(ILogger<Worker> logger, ILeschacoServices services, INotificarActividades actividades, IProcesosOperaciones operaciones)
        {
            _logger = logger;
            _services = services;
            _actividades = actividades;
            _operaciones = operaciones;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                stopwatch.Start();
                await Respuesta(stoppingToken); 
                stopwatch.Stop();
                _logger.LogInformation((stopwatch.ElapsedMilliseconds / 1000).ToString() + " Segundos");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {              
                _logger.LogError(ex, "Error durante la ejecuci�n del servicio.");
                Environment.Exit(0);
            }
        }

        private async Task Respuesta(CancellationToken cancellationToken)
        {
            try
            {
                //await GestionarActividades();
                await GestionarOperaciones();
            }
            catch(Exception ex) {
            
            }

        }

        protected async Task GestionarOperaciones()
        {
            try
            {
                //await _operaciones.EnviarArchivosLchFotograficos();
                await _operaciones.EnviarArchivosLch();
                //await _operaciones.CopiarArchivosFile();
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task GestionarActividades()
        {
            try
            {
                await _actividades.NotificarAtraso();
                await _actividades.NotificarArribo();
                await _actividades.NotificarDesconsolidacion();          
            }
            catch (Exception ex) 
            { 

            }
        }   
    }
}
