using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using FotoGol_Data;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace FotoGol_Email
{
    public class WorkerRole : RoleEntryPoint
    {
        private static bool storageInitialized = false;
        private static object gate = new Object();
        private static CloudBlobClient blobStorage;
        private static CloudQueueClient queueStorage;

        public override void Run()
        {
            Trace.WriteLine("FotoGol_Email entry point called", "Information");

            InitializeStorage();

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");

                var list = MailGetter.GetPhotoData(true);

                foreach (var item in list)
                {
                    //Subo la imagen al Blob Storage
                    CloudBlobContainer container = blobStorage.GetContainerReference("fotogolpics");
                    string uniqueBlobName = string.Format("image_{0}.jpg", Guid.NewGuid().ToString());
                    CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                    
                    //blob.Properties.ContentType = item.ImageType;
                    blob.UploadFromStream(new MemoryStream(item.Data));
                    System.Diagnostics.Trace.TraceInformation("Uploaded image '{0}' to blob storage as '{1}'", item.FileName, uniqueBlobName);

                    //Creo un registro en la tabla
                    FotoGolEntry entry = new FotoGolEntry() { GuestName = item.From, Message = item.Title, PhotoUrl = blob.Uri.ToString(), ThumbnailUrl = blob.Uri.ToString() };
                    FotoGolEntryDataSource ds = new FotoGolEntryDataSource();
                    ds.AddGuestBookEntry(entry);
                    System.Diagnostics.Trace.TraceInformation("Added entry {0}-{1} in table storage for guest '{2}'", entry.PartitionKey, entry.RowKey, entry.GuestName);

                    //Pongo un mensaje para procesar
                    var queue = queueStorage.GetQueueReference("fotogolthumbs");
                    var message = new CloudQueueMessage(String.Format("{0},{1},{2}", uniqueBlobName, entry.PartitionKey, entry.RowKey));
                    queue.AddMessage(message);
                    System.Diagnostics.Trace.TraceInformation("Queued message to process blob '{0}'", uniqueBlobName);
                    
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.GetConfigurationSettingValue(configName));
            });

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private void InitializeStorage()
        {
            if (storageInitialized)
            {
                return;
            }

            lock (gate)
            {
                if (storageInitialized)
                {
                    return;
                }

                try
                {
                    // read account configuration settings
                    var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

                    // create blob container for images
                    blobStorage = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobStorage.GetContainerReference("fotogolpics");
                    container.CreateIfNotExist();

                    // configure container for public access
                    var permissions = container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions(permissions);

                    // create queue to communicate with worker role
                    queueStorage = storageAccount.CreateCloudQueueClient();
                    CloudQueue queue = queueStorage.GetQueueReference("fotogolthumbs");
                    queue.CreateIfNotExist();
                }
                catch (WebException)
                {
                    throw new WebException("Storage services initialization failure. "
                        + "Check your storage account configuration settings. If running locally, "
                        + "ensure that the Development Storage service is running.");
                }

                storageInitialized = true;
            }
        }
 
    }
}
