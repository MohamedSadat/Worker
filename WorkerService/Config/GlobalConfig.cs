using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerService.Data;
using WorkerService.Services;
using WorkerService.Workers;

namespace WorkerService.Config
{
    public class GlobalConfiguration
    {
        public static WorkerAppState app = new WorkerAppState();
        public static List<LogModel> logs = new List<LogModel>();

        static GlobalConfiguration()
        {
            if(!File.Exists("appsetting.json"))
            {
                var xser = new SerializeService();
               Task.Run(()=> xser.Serialize(app));
            }
            else
            {
                var j = File.ReadAllText("appsetting.json");
                app = JsonSerializer.Deserialize<WorkerAppState>(j);
            }
    
           
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
  Host.CreateDefaultBuilder(args)
      .ConfigureServices((hostContext, services) =>
      {
          services.AddHostedService<DBWorker>();
          services.AddHostedService<MyBackgroundService>();
          services.AddHostedService<LogWorker>();
      });

    }
}
