using Newtonsoft.Json;
using ReviewsTracker.Contracts;
using ReviewsTracker.Model;
using ReviewsTracker.Storage.Contracts;
using ReviewTracker.Storage;
using ReviewTracker.Storage.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IStorageService _storageService;
        public TrackingService(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public async Task<Tracking> StartTracking(Tracking tracking)
        {
            var trackingId = Guid.NewGuid().ToString();
            var trackingRequest = new TrackingRequest()
            {
                Id = trackingId,
                Asins = tracking.Asins
            };
            await _storageService.QueueMessage(Constants.trackingQueue, JsonConvert.SerializeObject(trackingRequest));
            tracking.TrackingId = trackingId;
            return tracking;
        }
    }
}
