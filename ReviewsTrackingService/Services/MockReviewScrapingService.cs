using ReviewsTrackingService.Contracts;
using ReviewsTrackingService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTrackingService.Services
{
    public class MockReviewScrapingService : IReviewScrapingService
    {
        public async Task<IEnumerable<Review>> GetReviews(string asin)
        {
            await Task.Delay(2000);
            return GetProductReview(asin);
        }

        private IEnumerable<Review> GetProductReview(string asin)
        {
            for (int i = 0; i < 25; i++)
            {
                var review = new Review()
                {
                    Asin = asin,
                    Content = $"LoremIpsum - {i}",
                    Id = Guid.NewGuid().ToString()
                };
                yield return review;
            }
        }
    }
}
