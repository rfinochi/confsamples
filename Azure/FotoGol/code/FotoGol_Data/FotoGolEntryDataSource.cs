using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace FotoGol_Data
{
    public class FotoGolEntryDataSource
    {
        private static CloudStorageAccount storageAccount;
        private FotoGolDataContext context;

        static FotoGolEntryDataSource()
        {
            storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            CloudTableClient.CreateTablesFromModel(
                typeof(FotoGolDataContext),
                storageAccount.TableEndpoint.AbsoluteUri,
                storageAccount.Credentials);
        }

        public FotoGolEntryDataSource()
        {
            this.context = new FotoGolDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
        }

        public IEnumerable<FotoGolEntry> Select()
        {
            var results = from g in this.context.FotoGolEntry
                          where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                          select g;
            return results;
        }

        public void AddGuestBookEntry(FotoGolEntry newItem)
        {
            this.context.AddObject("FotoGolEntry", newItem);
            this.context.SaveChanges();
        }

        public void DeleteAll()
        {
            var results = Select();

            foreach (var item in results)
            {
                this.context.DeleteObject(item);
            }

            this.context.SaveChanges();
        }

        public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
        {
            var results = from g in this.context.FotoGolEntry
                          where g.PartitionKey == partitionKey && g.RowKey == rowKey
                          select g;

            var entry = results.FirstOrDefault<FotoGolEntry>();
            entry.ThumbnailUrl = thumbUrl;
            this.context.UpdateObject(entry);
            this.context.SaveChanges();
        }
    }
}