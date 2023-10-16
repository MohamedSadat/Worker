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
using WorkerService.Data;
using WorkerService.Services;

namespace WorkerService.Workers
{
    public sealed class DBWorker : BackgroundService
    {
        private readonly ILogger<DBWorker> _logger;

        //for signal completion
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public DBWorker(
            IHostApplicationLifetime hostApplicationLifetime, ILogger<DBWorker> logger) =>
        (_hostApplicationLifetime, _logger) = (hostApplicationLifetime, logger);
        //  _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MemoryCacheOptions opt = new MemoryCacheOptions();

            CacheService<LedgerTableModel> xcache = new CacheService<LedgerTableModel>(new MemoryCache(opt));
       //     Console.WriteLine($"DBThread {Thread.CurrentThread.ManagedThreadId}");

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
                _logger.LogInformation("DB Worker running at: {time}", DateTimeOffset.Now);

                var x = new COAQueryService(GlobalConfiguration.app);
                try
                {
                    var r = await x.GetLedgerAccountsAsync(new CG.Infrastructure.CGModels.CGFilters.LedgerAccountFilter { Company = GlobalConfiguration.app.Company.CoID });
                    Console.WriteLine($"COA Count. {r.Count}");
                    GlobalConfiguration.logs.Add(new LogModel { Message = $"COA Count. {r.Count}", LogDate = DateTime.Now });
                    if (items.Count == 0)
                    {
                        Console.WriteLine("Writing the cache");
                        xcache.SetCache(r);

                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"DB err {ex.Message}");
                }
            
                await Task.Delay(10_000, stoppingToken);

                // When completed, the entire app host will stop.
                //   _hostApplicationLifetime.StopApplication();
            }
        }

    }
}
