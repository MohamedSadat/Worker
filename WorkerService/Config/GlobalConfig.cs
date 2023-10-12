using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerService.Services;

namespace WorkerService.Config
{
    public class GlobalConfiguration
    {
        public static WorkerAppState app = new WorkerAppState();
        static GlobalConfiguration()
        {
            var j = File.ReadAllText("appsetting.json");
            app = JsonSerializer.Deserialize<WorkerAppState>(j);
            //   GlobalConfig.app = new WorkerAppState();
            // GlobalConfig.app.ConMain = "Server=kitchino.ddns.net;Database=Kitchino;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;password=Lotus6488;Persist Security Info=True;Integrated Security=false";
            // GlobalConfig.app.ConLog = "Server=kitchino.ddns.net;Database=CGService;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;password=Lotus6488;Persist Security Info=True;Integrated Security=false";
            //   GlobalConfig.app.CompanyId = "Test";
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
  Host.CreateDefaultBuilder(args)
      .ConfigureServices((hostContext, services) =>
      {
          services.AddHostedService<Worker>();
          services.AddHostedService<MyBackgroundService>();

      });

    }
}
