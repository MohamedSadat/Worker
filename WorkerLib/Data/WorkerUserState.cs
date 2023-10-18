using CG.Infrastructure.CGConfiguration;
using CG.Infrastructure.CGModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerLib.Data
{
    public class WorkerUserState : IUserState
    {
        public Users User { get; set; }
        public int SessionStarted { get; set; }
        public Stack<string> NavHistory { get; set; }
        public bool IndexLoaded { get; set; }
        public string LastVisitedPage { get; set; }
        public DateTime SessionDate { get; set; }
        public string UserId { get; set; }
    }
}
