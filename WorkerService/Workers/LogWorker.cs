using CG.GLModule.Services;
using CG.Infrastructure.CGModels;
using CG.Infrastructure.System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerService.Config;
using WorkerService.Data;
using WorkerService.Services;

namespace WorkerService.Workers
{
    internal class LogWorker : BackgroundService
    {
        private readonly ILogger<DBWorker> _logger;

        //for signal completion
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public LogWorker(IHostApplicationLifetime hostApplicationLifetime, ILogger<DBWorker> logger) =>
        (_hostApplicationLifetime, _logger) = (hostApplicationLifetime, logger);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Logging Worker running at: {time}", DateTimeOffset.Now);
                foreach (var address in GlobalConfiguration.app.Servers)
                {
                    if (await MonitorService.CheckReachability(address) == true)
                    {
                        GlobalConfiguration.logs.Add(new LogModel { Message = $"{address.ServerName} is Healthy", LogDate = DateTime.Now,ErrorCode=200 });

                    }
                    else
                    {
                      
                        GlobalConfiguration.logs.Add(new LogModel { Message = $"{address.ServerName} is not reachable", LogDate = DateTime.Now,ErrorCode=500 });
                    }
                }

                string fileName = "log.json";
                using FileStream createStream = File.Create(fileName);
                var options = new JsonSerializerOptions { WriteIndented = true };
                await JsonSerializer.SerializeAsync(createStream, GlobalConfiguration.logs, options);
                await createStream.DisposeAsync();
                await Task.Delay(20_000, stoppingToken);

                // When completed, the entire app host will stop.
                //   _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
