using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerLib.Data
{
    public class CachWorkerSetting
    {
        public CachWorkerSetting()
        {
                
        }
        public bool CacheUpdated { get; set; } = false;
        public List<LogModel> Logs { get; set; }
    }
}
