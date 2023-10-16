using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Cdal;
using CG.GLModule.Services.CompanyServices;
using CG.GLModule.Services;
using CG.Infrastructure.System;
using System.Text.Json;
using WorkerService.Config;
using WorkerService.Data;
using CG.Infrastructure.CGConfiguration;
using WorkerService.Services;
using WorkerService.Workers;

namespace WorkerService
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            try
            {

                var xcompany = new CompanyQueryService(GlobalConfiguration.app);
                GlobalConfiguration.app.Company = await xcompany.GetCompany("Test");
                Cdal.GlobalConfig.comodel = GlobalConfiguration.app.Company;

                //GlobalConfiguration.builder = GlobalConfiguration.CreateHostBuilder(args);
                GlobalConfiguration.appbuilder = GlobalConfiguration.CreateApptBuilder(args);
                GlobalConfiguration.host = GlobalConfiguration.appbuilder.Build();

                // GlobalConfiguration.host=GlobalConfiguration.builder.Build();
                await GlobalConfiguration.host.RunAsync();


                Console.WriteLine("Hello, World!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            //HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            //builder.Services.AddSingleton<MonitorLoop>();
            //builder.Services.AddHostedService<QueuedHostedService>();
            //builder.Services.AddSingleton<IBackgroundTaskQueue>(_ =>
            //{
            //    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
            //    {
            //        queueCapacity = 100;
            //    }

            //    return new DefBackQueue(queueCapacity);
            //});

            //IHost host = builder.Build();

            //MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
            //monitorLoop.StartMonitorLoop();

            //host.Run();

        }





    }
}