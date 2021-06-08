using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace Azure.Table.Storage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Table Storage");

            const string storageConnectionString = "CONNECTION STRING";
            const string tableName = "TABLE NAME";

            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var table = tableClient.GetTableReference(tableName);


            var data = new TransactionData(Guid.NewGuid().ToString(), "{\"key\":\"value\"}");
            Merge(table, data).Wait();
            Query(table, data.RowKey).Wait();
            Delete(table, data.RowKey).Wait();
        }

        public static async Task Merge(CloudTable table, TransactionData transaction)
        {
            var insertOrMergeOperation = TableOperation.InsertOrMerge(transaction);

            // Execute the operation.
            var result = await table.ExecuteAsync(insertOrMergeOperation);
            var insertedData = result.Result as TransactionData;

            Console.WriteLine("Added transaction.");
        }

        public static async Task Query(CloudTable table, string key)
        {
            var retrieveOperation = TableOperation.Retrieve<TransactionData>("guid", key);

            var result = await table.ExecuteAsync(retrieveOperation);
            var data = result.Result as TransactionData;

            if (data != null)
            {
                Console.WriteLine("Fetched \t{0}\t{1}\t{2}",
                    data.PartitionKey, data.RowKey, data.Data);
            }
        }

        public static async Task Delete(CloudTable table, string key)
        {
            var retrieveOperation = TableOperation.Retrieve<TransactionData>("guid", key);

            var result = await table.ExecuteAsync(retrieveOperation);
            var data = result.Result as TransactionData;
            
            var deleteOperation = TableOperation.Delete(data);
            await table.ExecuteAsync(deleteOperation);
            
            Console.WriteLine("Deleted transaction.");
        }


    }
}