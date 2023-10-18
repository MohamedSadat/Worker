using CG.GLModule.ViewModels.Journal;
using CG.Infrastructure.CGModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerLib.Data;
using WorkerLib.Services;
using WorkerLib.Workers;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class WorkerConfigExtensions
    {

        public static IServiceCollection AddCacheWorkerDependencyGroup(
           this IServiceCollection services)
        {
            services.AddHostedService<DBCacheWorker>();
           
            services.AddSingleton<CachWorkerSetting>();
            services.AddSingleton<CacheService>();

            return services;
        }

   


    }
}
