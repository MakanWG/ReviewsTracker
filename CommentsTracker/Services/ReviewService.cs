using ReviewsTracker.Contracts;
using ReviewsTracker.Model;
using ReviewsTracker.Storage.Contracts;
using ReviewTracker.Storage;
using ReviewTracker.Storage.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IStorageService _storageService;
        public ReviewService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<IEnumerable<Review>> GetTrackedReviews(string trackingId)
        {
            var reviews = await _storageService.GetTableData<TrackedReview>(Constants.trackedReviewsTable, trackingId);
            return reviews.Select(trackedReview => new Review() { Asin = trackedReview.ProductAsin, Content = trackedReview.Content});
        }
    }
}
