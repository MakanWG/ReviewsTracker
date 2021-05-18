using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewsTracker.Contracts;
using ReviewsTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReviewsTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("/tracked")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Review>))]
        public async Task<ActionResult<IEnumerable<Review>>> Get([FromQuery] string trackingId)
        {
            var reviews = await _reviewService.GetTrackedReviews(trackingId);
            return Ok(reviews);
        }
    }
}
