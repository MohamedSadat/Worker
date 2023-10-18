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
using WorkerLib.Data;

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

               var builder = Host.CreateApplicationBuilder();
                builder.Services.AddCacheWorkerDependencyGroup();
                builder.Services.AddSingleton<IAppState,WorkerLib.Data.WorkerAppState>();
                builder.Services.AddSingleton<CachWorkerSetting>();
                var app=builder.Build();
                
                //init app state
                using (var serviceScope = app.Services.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;

                    var myappdep = services.GetRequiredService<IAppState>();
                    myappdep.itemModels = new List<CG.Infrastructure.CGModels.ItemModel>();
                    myappdep.Company = GlobalConfiguration.app.Company;
                    myappdep.ConMain = GlobalConfiguration.app.ConMain;
                    myappdep.ConLog = GlobalConfiguration.app.ConLog;

                      var cachesetting = services.GetRequiredService<CachWorkerSetting>();
                      cachesetting.Logs = new List<WorkerLib.Data.LogModel>();

                }
                //return control to main thread
                 Task.Run(()=> app.RunAsync());


                Console.WriteLine("Hello, World!");
              var k=  Console.ReadLine();
                if(k=="q")
                {
                  await  app.StopAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }





    }
}