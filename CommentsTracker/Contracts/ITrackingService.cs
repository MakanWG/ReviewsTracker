using ReviewsTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Contracts
{
    public interface ITrackingService
    {
        Task<Tracking> StartTracking(Tracking tracking); 
    }
}
