using ReviewsTracker.Model;
using ReviewTracker.Storage.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Contracts
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetTrackedReviews(string trackingId);
    }
}
