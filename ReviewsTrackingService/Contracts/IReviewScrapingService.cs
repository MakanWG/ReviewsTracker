using ReviewsTrackingService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTrackingService.Contracts
{
    public interface IReviewScrapingService
    {
        Task<IEnumerable<Review>> GetReviews(string asin);
    }
}
