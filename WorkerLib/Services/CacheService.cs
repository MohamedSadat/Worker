using CG.Infrastructure.CGModels;
using CG.Infrastructure.System;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerLib.Services
{
    public class CacheService
    {
        private readonly IMemoryCache memory;

        public CacheService(IMemoryCache _memory)
        {
            memory = _memory;
        }
        public void SetCache(List<ItemModel> items)
        {

            if (memory != null)
            {
                memory.Set($"items", items, TimeSpan.FromMinutes(20));
            }


        }
        public List<ItemModel> ReadCache()
        {
            List<ItemModel> items = new List<ItemModel>();
            if (memory != null)
            {
                items = memory.Get<List<ItemModel>>($"items");
                if (items != null)
                {
                    return items;
                }
                else
                    return new List<ItemModel>();
            }
            else
            { return items; }
        }
    }
}
