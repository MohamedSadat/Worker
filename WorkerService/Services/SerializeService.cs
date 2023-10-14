using CG.Infrastructure.CGConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerService.Config;
using WorkerService.Data;

namespace WorkerService.Services
{
    internal class SerializeService
    {
        public async Task Serialize(IAppState app)
        {
  
            string fileName = "appsetting.json";
            using FileStream createStream = File.Create(fileName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(createStream, app, options);
            await createStream.DisposeAsync();
        }
        public IAppState Deserialize()
        {
            var j = File.ReadAllText("appsetting.json");
            var app = JsonSerializer.Deserialize<WorkerAppState>(j);
            return app;
        }
    }
}
