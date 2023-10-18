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
using WorkerLib.Services;

using WorkerLib.Data;
using CG.Infrastructure.CGConfiguration;
using CG.InvModule.Services;
using CG.Infrastructure.CGModels.CGFilters;
using System.Text.Json;

namespace WorkerLib.Workers
{
    public sealed class DBCacheWorker : BackgroundService
    {
        private readonly ILogger<DBCacheWorker> _logger;
        private readonly IAppState app;
        private readonly CachWorkerSetting log;

        //for signal completion
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public DBCacheWorker(
            IHostApplicationLifetime hostApplicationLifetime, ILogger<DBCacheWorker> logger, IAppState app, CachWorkerSetting log) =>
        (_hostApplicationLifetime, _logger, this.app, this.log) = (hostApplicationLifetime, logger, app, log);
      

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MemoryCacheOptions opt = new MemoryCacheOptions();

           var xcache = new CacheService(new MemoryCache(opt));
            var xquery=new  ItemsQueryService(app,null,null); 
          
                _logger.LogInformation("DB Cache Worker running at: {time}", DateTimeOffset.Now);
            log.Logs.Add(new LogModel { Message = "DB Cache Worker running at", LogDate = DateTime.Now });

            while (!stoppingToken.IsCancellationRequested)
            {
                //Items not loaded
                if (app.itemModels.Count == 0)
                {
                    app.itemModels = await xquery.GetFlatAsync();
                    log.Logs.Add(new LogModel { Message = $"Items {app.itemModels.Count}", LogDate = DateTime.Now });
                    try
                    {
                        xcache.SetCache(app.itemModels);

                    }
                    catch (Exception ex)
                    {
                        log.Logs.Add(new LogModel { Message = $"Items {app.itemModels.Count}", LogDate = DateTime.Now });

                    }
                }
                else
                {
                    try
                    {
               
                        var count = xquery.GetRecCountByFilter(new ItemFilter { Company = app.Company.CoID });
                        log.Logs.Add(new LogModel { Message = $"Item count {count}", LogDate = DateTime.Now });

                        //Check if cache is updated
                        if (count != app.itemModels.Count)
                        {
                            app.itemModels = await xquery.GetFlatAsync();
                            xcache.SetCache(app.itemModels);
                            log.CacheUpdated = true;
                            log.Logs.Add(new LogModel { Message = "Cache Updated", LogDate = DateTime.Now });
                        }
                    }
                    catch(Exception ex)
                    {
                        log.Logs.Add(new LogModel { Message = $"Error {ex.Message}", LogDate = DateTime.Now });

                    }
                }
               await SaveJson();
                await Task.Delay(20_000, stoppingToken);

                // When completed, the entire app host will stop.
                //   _hostApplicationLifetime.StopApplication();
            }
        }
        protected  async Task SaveJson()
        {
            
                string fileName = "log.json";
                using FileStream createStream = File.Create(fileName);
                var options = new JsonSerializerOptions { WriteIndented = true };

                await JsonSerializer.SerializeAsync(createStream, log.Logs, options);
                await createStream.DisposeAsync();
            
                // When completed, the entire app host will stop.
                //   _hostApplicationLifetime.StopApplication();
            
        }

    }
}
