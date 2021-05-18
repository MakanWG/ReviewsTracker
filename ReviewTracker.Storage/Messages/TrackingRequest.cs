using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewTracker.Storage.Messages
{
    public class TrackingRequest
    {
        public string Id { get; set; }
        public IEnumerable<string> Asins { get; set; }
    }
}
