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

using WorkerLib.Data;
using WorkerLib.Services;

namespace WorkerLib.Workers
{
    public sealed class LogWorker : BackgroundService
    {
        private readonly ILogger<DBCacheWorker> _logger;
        private readonly CachWorkerSetting log;

        //for signal completion
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
      
        public LogWorker(IHostApplicationLifetime hostApplicationLifetime, ILogger<DBCacheWorker> logger, CachWorkerSetting log)
        {
            this._hostApplicationLifetime = hostApplicationLifetime;
            this._logger = logger;
            this.log = log;
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
            _logger.LogInformation("Logging Worker running at: {time}  ", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
              
                string fileName = "log.json";
                using FileStream createStream = File.Create(fileName);
                var options = new JsonSerializerOptions { WriteIndented = true };
             
                await JsonSerializer.SerializeAsync(createStream, log.Logs, options);
                await createStream.DisposeAsync();
                await Task.Delay(20_000, stoppingToken);

                // When completed, the entire app host will stop.
                //   _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
