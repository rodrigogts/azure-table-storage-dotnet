using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Table.Storage
{
    class TransactionData : TableEntity
    {
        public string Data { get; set; }

        public TransactionData()
        {
            PartitionKey = "guid";
        }
        
        public TransactionData(string key)
        {
            PartitionKey = "guid";
            RowKey = key;
        }

        public TransactionData(string key, string data)
        {
            PartitionKey = "guid";
            RowKey = key;
            Data = data;
        }

    }
}
