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

namespace WorkerService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
         try
            {
           
                //   GlobalConfiguration.app.Servers.Add("k");

                var xcompany = new CompanyQueryService(GlobalConfiguration.app);
                GlobalConfiguration.app.Company = await xcompany.GetCompany("Test");

                Cdal.GlobalConfig.comodel = GlobalConfiguration.app.Company;
                Console.WriteLine(GlobalConfiguration.app.Company.CoName);
               // var j = JsonSerializer.Serialize(GlobalConfiguration.app);
              //  File.WriteAllText("appsetting.json", j);

                GlobalConfiguration.CreateHostBuilder(args).Build().Run();
                Console.WriteLine("Hello, World!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
     
        }

  


    }
}