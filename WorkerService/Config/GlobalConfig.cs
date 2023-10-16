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
        public static IHost? host;
        public static IHostBuilder builder;
        public static HostApplicationBuilder appbuilder;

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
.ConfigureServices(
(hostContext, services) =>
{
  services.AddHostedService<DBWorker>();
  services.AddHostedService<MyBackgroundService>();
  services.AddHostedService<LogWorker>();
  services.AddSingleton<IMonitorService, MonitorService>();
  services.AddSingleton<SerializeService>();
  services.AddMemoryCache();


});
        public static HostApplicationBuilder CreateApptBuilder(string[] args)
        {

            appbuilder = Host.CreateApplicationBuilder(args);
            appbuilder.Services.AddHostedService<DBWorker>();
            appbuilder.Services.AddHostedService<MyBackgroundService>();
            appbuilder.Services.AddHostedService<LogWorker>();
            appbuilder.Services.AddSingleton<IMonitorService, MonitorService>();
            appbuilder.Services.AddSingleton<SerializeService>();
            appbuilder.Services.AddMemoryCache();
            return appbuilder;

        }


    }
}
