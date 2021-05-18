using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReviewsTracker.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewsTracker.Storage.Services
{
    public class AzureStorageService : IStorageService
    {
        private const string connectionStringName = "storageConnectionString";
        private readonly string _connectionString;
        private readonly ILogger<AzureStorageService> _logger;
        public AzureStorageService(IConfiguration configuration, ILogger<AzureStorageService> logger)
        {
            _connectionString = configuration.GetConnectionString(connectionStringName);
            _logger = logger;
        }

        public async Task QueueMessage(string queueName, string message)
        {
            var queueClient = new QueueClient(_connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();

            await queueClient.SendMessageAsync(message);
            _logger.LogInformation($"Queued {message}");
        }

        public async Task<QueueMessage> ReadNextMessage(string queueName)
        {
            var queueClient = new QueueClient(_connectionString, queueName);
            await queueClient.CreateIfNotExistsAsync();
            var receivedMessage = await queueClient.ReceiveMessageAsync();
            return receivedMessage.Value;
        }

        public async Task DequeueMessage(string queueName, QueueMessage message)
        {
            var queueClient = new QueueClient(_connectionString, queueName);
            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        public async Task<IEnumerable<T>> GetTableData<T>(string tableName, string partitionKey) where T : TableEntity, new()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var client = storageAccount.CreateCloudTableClient();
            var tableReference = client.GetTableReference(tableName);
            await tableReference.CreateIfNotExistsAsync();
            var tableQuery = new TableQuery<T>()
            {
                FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey)
            };
            TableContinuationToken token = null;
            var resultSegment = await tableReference.ExecuteQuerySegmentedAsync<T>(tableQuery, token);
            return resultSegment.Results;
        }

        public async Task InsertTableData<T>(string tableName, string partitionKey, IEnumerable<T> data) where T : TableEntity
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var client = storageAccount.CreateCloudTableClient();
            var tableReference = client.GetTableReference(tableName);
            await tableReference.CreateIfNotExistsAsync();
            var insertOperation = new TableBatchOperation();
            foreach (var entity in data)
            {
                insertOperation.InsertOrReplace(entity);
            }
            await tableReference.ExecuteBatchAsync(insertOperation);
        }
    }
}
