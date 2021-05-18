using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReviewsTracker.Storage.Contracts;
using ReviewsTrackingService.Contracts;
using ReviewsTrackingService.Entities;
using ReviewTracker.Storage;
using ReviewTracker.Storage.Messages;
using ReviewTracker.Storage.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReviewsTrackingService
{
    public class ReviewTrackingService : IHostedService, IDisposable
    {
        
        private readonly ILogger<ReviewTrackingService> _logger;
        private readonly IReviewScrapingService _reviewScrapingService;
        private readonly IStorageService _storageService;
        private Timer _timer;

        public ReviewTrackingService(ILogger<ReviewTrackingService> logger,
            IReviewScrapingService reviewScrapingService,
            IStorageService storageService)
        {
            _logger = logger;
            _reviewScrapingService = reviewScrapingService;
            _storageService = storageService;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Tracking Service running.");

            _timer = new Timer(async _ => await MonitorQueue(_), null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15));

            _logger.LogInformation("Started Tracking Service running.");
            return Task.CompletedTask;
        }

        private async Task MonitorQueue(object state)
        {
            _logger.LogInformation("checking queue");
            var message = await _storageService.ReadNextMessage(Constants.trackingQueue);
            if (message != null)
            {
                _logger.LogInformation("Got message");
                await _storageService.DequeueMessage(Constants.trackingQueue, message);
                var trackingRequest = message.Body.ToObjectFromJson<TrackingRequest>();
                _logger.LogInformation("checking queue");
                var reviewsBatch = new List<Review>();
                foreach (var productAsin in trackingRequest.Asins)
                {
                    _logger.LogInformation($"loading reviews for product{productAsin}");
                    var reviews = await _reviewScrapingService.GetReviews(productAsin);
                    reviewsBatch.AddRange(reviews);
                }
                await StoreReviews(reviewsBatch, trackingRequest.Id);
                await _storageService.QueueMessage(Constants.statusQueue, trackingRequest.Id);
            }
        }

        private async Task StoreReviews(IEnumerable<Review> reviews, string trackingId)
        {
            var tableReviews = reviews.Select(review => new TrackedReview(trackingId, review.Id, review.Content, review.Asin));
            await _storageService.InsertTableData(Constants.trackedReviewsTable, trackingId, tableReviews);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Tracking Service");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
