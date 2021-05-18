using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Model
{
    public class Tracking
    {
        public string TrackingId { get; set; }
        public IEnumerable<string> Asins { get; set; }
    }
}
