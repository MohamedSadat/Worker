using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HoastedServiceLib
{
    public class TimedBackgroundService : BackgroundService
    {
        private readonly ILogger<TimedBackgroundService> _logger;
        private readonly ICounterService counterService;

        public TimedBackgroundService(ILogger<TimedBackgroundService> logger, ICounterService counterService)
        {
            _logger = logger;
            this.counterService = counterService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                counterService.Increment();
                _logger.LogInformation("Background work running at: {time}", counterService.CurrentCount);

                // Simulate work by delaying for 10 seconds
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("CounterBackgroundService is stopping.");
        }
    }
}
