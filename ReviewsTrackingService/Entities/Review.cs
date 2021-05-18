using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTrackingService.Entities
{
    public class Review
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Asin { get; set; }
    }
}
