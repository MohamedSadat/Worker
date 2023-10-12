using CG.Infrastructure.CGConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerService.Config;

namespace WorkerService.Services
{
    internal class WorkerSerializer
    {
        public void Serialize(IAppState app)
        {
            var j = JsonSerializer.Serialize(app);
            File.WriteAllText("appsetting.json", j);
        }
        public IAppState Deserialize()
        {
            var j = File.ReadAllText("appsetting.json");
            var app = JsonSerializer.Deserialize<WorkerAppState>(j);
            return app;
        }
    }
}
