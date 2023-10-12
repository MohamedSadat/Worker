using CG.GLModule.Services;
using CG.Infrastructure.CGModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Config;

namespace WorkerService.Services
{
    public sealed class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        //for signal completion
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public Worker(
            IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger) =>
        (_hostApplicationLifetime, _logger) = (hostApplicationLifetime, logger);
        //  _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MemoryCacheOptions opt = new MemoryCacheOptions();

            WorkerCache<LedgerTableModel> xcache = new WorkerCache<LedgerTableModel>(new MemoryCache(opt));

            while (!stoppingToken.IsCancellationRequested)
            {
                //   MemoryCacheOptions opt = new MemoryCacheOptions();

                //   WorkerCache<LedgerTableModel> xcache = new WorkerCache<LedgerTableModel>(new MemoryCache(opt));
                var items = xcache.ReadCache();
                if (items.Count == 0)
                {
                    Console.WriteLine("Cache is empty");
                }
                else
                {
                    Console.WriteLine("Cache Not empty");

                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var x = new COAQueryService(GlobalConfiguration.app);
                var r = await x.GetLedgerAccountsAsync(new CG.Infrastructure.CGModels.CGFilters.LedgerAccountFilter { Company = GlobalConfiguration.app.Company.CoID });
                Console.WriteLine($"COA Count. {r.Count}");
                if (items.Count == 0)
                {
                    Console.WriteLine("Writing the cache");
                    xcache.SetCache(r);

                }
                await Task.Delay(8_000, stoppingToken);

                // When completed, the entire app host will stop.
                //   _hostApplicationLifetime.StopApplication();
            }
        }

    }
}
