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
