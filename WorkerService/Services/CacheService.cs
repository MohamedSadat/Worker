using CG.Infrastructure.CGModels;
using CG.Infrastructure.System;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Services
{
    public class CacheService<T> where T : class
    {
        private readonly IMemoryCache memory;

        public CacheService(IMemoryCache _memory)
        {
            memory = _memory;
        }
        public void SetCache(List<T> items)
        {

            if (memory != null)
            {
                memory.Set($"items", items, TimeSpan.FromMinutes(20));
            }


        }
        public List<T> ReadCache()
        {
            List<T> items = new List<T>();
            if (memory != null)
            {
                items = memory.Get<List<T>>($"items");
                if (items != null)
                {
                    return items;
                }
                else
                    return new List<T>();
            }
            else
            { return items; }
        }
    }
}
