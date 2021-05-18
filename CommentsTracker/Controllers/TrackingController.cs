using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewsTracker.Contracts;
using ReviewsTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingService;
        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Review>))]
        public async Task<ActionResult<Tracking>> Post([FromBody] Tracking tracking)
        {
            var initiatedTracking = await _trackingService.StartTracking(tracking);
            return Created($"{Request.Path}/{initiatedTracking.TrackingId}", initiatedTracking);
        }
    }
}
