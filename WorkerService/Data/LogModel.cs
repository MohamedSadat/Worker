using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Data
{
    public class LogModel
    {
        public DateTime LogDate { get; set; }=DateTime.Now;
        public string Message { get; set; } = "";
        public int ErrorCode { get; set; } = 200;
    }
}
