using Microsoft.EntityFrameworkCore;
using System.Text;
using WSArchivosLCH;
using WSArchivosLCH.Abstractions;
using WSArchivosLCH.Models;
using WSArchivosLCH.Operaciones;
using WSArchivosLCH.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ILeschacoServices, Services>();
        services.AddSingleton<INotificarActividades, GestionarActividades>();
        services.AddSingleton<IProcesosOperaciones, GestionarOperaciones>();

        //services.AddHttpClient("OwnData", client =>
        //{
        //    client.BaseAddress = new Uri("https://localhost:7270/api/LCH_OD/");
        //    client.Timeout = TimeSpan.FromSeconds(2000);
        //    client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IlVzZXIxQ3kiLCJzdWIiOiJiYXNlV2ViQXBpU3ViamVjdCIsImp0aSI6IjhlNzVlM2U5LThiN2QtNGUxZS04Mzc5LTJkODQwNTlkYjllMiIsImV4cCI6MTc2MDcxMjQxMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6ODQiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo4NCJ9.5IvfGrt8mYi6WrYllWEVu9TRNw0uFrABDig-ANSVO5c");
        //});

        services.AddHttpClient("OwnData", client =>
        {
            client.BaseAddress = new Uri("https://apigol.cargoamc.com/api/LCH_OD/");
            client.Timeout = TimeSpan.FromSeconds(1000);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IlVzZXIxQ3kiLCJzdWIiOiJiYXNlV2ViQXBpU3ViamVjdCIsImp0aSI6IjhlNzVlM2U5LThiN2QtNGUxZS04Mzc5LTJkODQwNTlkYjllMiIsImV4cCI6MTc2MDcxMjQxMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6ODQiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo4NCJ9.5IvfGrt8mYi6WrYllWEVu9TRNw0uFrABDig-ANSVO5c");
        });


        services.AddHttpClient("ApiLeschaco", client =>
        {
            client.BaseAddress = new Uri("https://colsys.com.co/ws/fam/IntegracionFamWS/");

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml"));
            client.DefaultRequestHeaders.Add("SOAPAction", "https://colsys.com.co/ws/fam/IntegracionFamWS/");

            var username = "gol";
            var password = "GV@3So96wl_5";
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                }
            };

        });

        //services.AddHttpClient("ApiLeschaco", client =>
        //{
        //    client.BaseAddress = new Uri("https://190.158.6.132:8443/ws/fam/IntegracionFamWS/");

        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml"));
        //    client.DefaultRequestHeaders.Add("SOAPAction", "https://190.158.6.132:8443/ws/fam/IntegracionFamWS/");

        //    var username = "seidor";
        //    var password = "=Ye7zdT5u8$SDt#V";
        //    var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
        //}).ConfigurePrimaryHttpMessageHandler(() =>
        //{
        //    return new HttpClientHandler
        //    {
        //        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        //        {
        //            return true;
        //        }
        //    };

        //});
    })
    .Build();

await host.RunAsync();
