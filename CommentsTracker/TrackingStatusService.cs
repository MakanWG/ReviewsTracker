using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReviewsTracker.Storage.Contracts;
using ReviewTracker.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReviewsTracker
{
    public class TrackingStatusService : IHostedService, IDisposable
    {
        private readonly ILogger<TrackingStatusService> _logger;
        private readonly IStorageService _storageService;
        private Timer _timer;
        public TrackingStatusService(ILogger<TrackingStatusService> logger,
            IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting tracking status monitoring.");

            _timer = new Timer(async _ => await MonitorQueue(_), null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15));

            _logger.LogInformation("Started tracking status monitoring.");
            return Task.CompletedTask;
        }

        private async Task MonitorQueue(object state)
        {
            _logger.LogInformation("checking queue");
            var message = await _storageService.ReadNextMessage(Constants.statusQueue);
            if (message != null)
            {
                await _storageService.DequeueMessage(Constants.statusQueue, message);
                var trackingId = message.Body.ToString();
                _logger.LogInformation($"tracking complete for id {trackingId}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping tracking status monitoring");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
