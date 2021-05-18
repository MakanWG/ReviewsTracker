using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewTracker.Storage.TableEntities
{
    public class TrackedReview : TableEntity
    {
        public TrackedReview()
        {

        }
        public TrackedReview(string partitionkey, string rowkey, string content, string asin)
        {
            PartitionKey = partitionkey;
            RowKey = rowkey;
            Content = content;
            ProductAsin = asin;
        }

        public string Content { get; set; }
        public string ProductAsin { get; set; }
    }
}
