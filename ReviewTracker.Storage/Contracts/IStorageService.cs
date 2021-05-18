using Azure.Storage.Queues.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Storage.Contracts
{
    public interface IStorageService
    {
        Task QueueMessage(string queueName, string message);
        Task<QueueMessage> ReadNextMessage(string queueName);
        Task DequeueMessage(string queueName, QueueMessage message);
        Task<IEnumerable<T>> GetTableData<T>(string tableName, string partitionKey) where T : TableEntity, new();
        Task InsertTableData<T>(string tableName, string partitionKey, IEnumerable<T> data) where T : TableEntity;
    }
}
