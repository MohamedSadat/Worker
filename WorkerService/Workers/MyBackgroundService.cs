using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService.Workers
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<DBWorker> _logger;
        public MyBackgroundService(
         IHostApplicationLifetime hostApplicationLifetime, ILogger<DBWorker> logger) =>
     ( _logger) = ( logger);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Temp Worker running at: {time}", DateTimeOffset.Now);


                await Task.Delay(9000, stoppingToken);  // Delay 1 second, and observe the cancellation token.
            }
        }
    }
}