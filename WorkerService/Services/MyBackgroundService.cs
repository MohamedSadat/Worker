using Microsoft.Extensions.Hosting;

namespace WorkerService.Services
{
    public class MyBackgroundService : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                Console.WriteLine($"My worker started");

                await Task.Delay(3000, stoppingToken);  // Delay 1 second, and observe the cancellation token.
            }
        }
    }
}