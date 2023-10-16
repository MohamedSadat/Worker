using CG.GLModule.Services;
using CG.Infrastructure.CGModels;
using CG.Infrastructure.System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
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
    public sealed class LogWorker : BackgroundService
    {
        private readonly ILogger<DBWorker> _logger;

        //for signal completion
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IMonitorService _monitorService;
        public LogWorker(IHostApplicationLifetime hostApplicationLifetime, ILogger<DBWorker> logger, IMonitorService monitorService)
        {
            this._hostApplicationLifetime = hostApplicationLifetime;
            this._logger = logger;
            this._monitorService = monitorService;
          _hostApplicationLifetime.ApplicationStopping.Register(OnShutdown);
            _hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            _hostApplicationLifetime.ApplicationStopped.Register(OnStopped);
        }
        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
        }
        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
        private void OnShutdown()
        {
            _logger.LogInformation("OnShutdown has been called.");
        }


      //  (_hostApplicationLifetime, _logger, _monitorService) = (hostApplicationLifetime, logger, monitorService);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
              
                _logger.LogInformation("Logging Worker running at: {time}  ", DateTimeOffset.Now);
             //   Console.WriteLine($"Log thread {Thread.CurrentThread.ManagedThreadId}");

                foreach (var address in GlobalConfiguration.app.Servers)
                {
//                    if (await _monitorService.CheckReachability(address) == true)
                        //Using Task.Run to run the method on a background thread
                        if (await Task.Run(()=> _monitorService.CheckReachability(address)) == true)

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
