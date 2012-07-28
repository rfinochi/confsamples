using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;

using FotoGol_Data;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace FotoGol_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private static CloudQueue queue;
        private static CloudBlobContainer container;

        public override void Run( )
        {
            Trace.TraceInformation( "Listening for queue messages..." );
            while ( true )
            {
                try
                {
                    CloudQueueMessage msg = queue.GetMessage( );
                    if ( msg != null )
                    {
                        string[] messageParts = msg.AsString.Split( new char[] { ',' } );
                        string uri = messageParts[ 0 ];
                        string partitionKey = messageParts[ 1 ];
                        string rowkey = messageParts[ 2 ];
                        Trace.TraceInformation( "Processing image in blob '{0}'.", new object[] { uri } );

                        CloudBlockBlob imageBlob = container.GetBlockBlobReference( uri );
                        MemoryStream image = new MemoryStream( );
                        imageBlob.DownloadToStream( image );
                        image.Seek( 0L, SeekOrigin.Begin );

                        string thumbnailUri = Path.GetFileNameWithoutExtension( uri ) + "_thumb.jpg";
                        CloudBlockBlob thumbnailBlob = container.GetBlockBlobReference( thumbnailUri );
                        thumbnailBlob.UploadFromStream( CreateThumbnail( image ) );

                        var dataSource = new FotoGolEntryDataSource( );
                        dataSource.UpdateImageThumbnail( partitionKey, rowkey, thumbnailBlob.Uri.AbsoluteUri );

                        queue.DeleteMessage( msg );

                        Trace.TraceInformation( "Generated thumbnail in blob '{0}'.", new object[] { thumbnailBlob.Uri } );
                    }
                    else
                    {
                        Thread.Sleep( 0x3e8 );
                    }
                }
                catch ( StorageClientException e )
                {
                    Trace.TraceError( "Exception when processing queue item. Message: '{0}'", e.Message );
                    System.Threading.Thread.Sleep( 5000 );
                }
            }
        }

        public override bool OnStart( )
        {
            DiagnosticMonitor.Start( "DiagnosticsConnectionString" );

            RoleEnvironment.Changing += new EventHandler<RoleEnvironmentChangingEventArgs>( RoleEnvironmentChanging );

            CloudStorageAccount.SetConfigurationSettingPublisher( ( configName, configSetter ) => configSetter( RoleEnvironment.GetConfigurationSettingValue( configName ) ) );
            CloudStorageAccount storageAccount = CloudStorageAccount.FromConfigurationSetting( "DataConnectionString" );

            container = storageAccount.CreateCloudBlobClient( ).GetContainerReference( "fotogolpics" );
            queue = storageAccount.CreateCloudQueueClient( ).GetQueueReference( "fotogolthumbs" );

            Trace.TraceInformation( "Creating container and queue..." );

            bool storageInitialized = false;
            while ( !storageInitialized )
            {
                try
                {
                    container.CreateIfNotExist( );
                    BlobContainerPermissions permissions = container.GetPermissions( );
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions( permissions );

                    queue.CreateIfNotExist( );
                    storageInitialized = true;
                }
                catch ( StorageClientException e )
                {
                    if ( e.ErrorCode != StorageErrorCode.TransportError )
                        throw;

                    Trace.TraceError( "Storage services initialization failure. Check your storage account configuration settings. If running locally, ensure that the Development Storage service is running. Message: '{0}'", new object[] { e.Message } );
                    Thread.Sleep( 5000 );
                }
            }
            return base.OnStart( );
        }

        private void RoleEnvironmentChanging( object sender, RoleEnvironmentChangingEventArgs e )
        {
            if ( e.Changes.Any( x => x is RoleEnvironmentConfigurationSettingChange ) )
                e.Cancel = true;
        }

        private Stream CreateThumbnail( Stream input )
        {
            int height;
            int width;
            Bitmap orig = new Bitmap( input );

            if ( orig.Width > orig.Height )
            {
                width = 0x80;
                height = (int)Math.Round( (double)( ( (double)( 0x80 * orig.Height ) ) / ( (double)orig.Width ) ) );
            }
            else
            {
                height = 0x80;
                width = (int)Math.Round( (double)( ( (double)( 0x80 * orig.Width ) ) / ( (double)orig.Height ) ) );
            }

            Bitmap thumb = new Bitmap( width, height );

            using ( Graphics graphic = Graphics.FromImage( thumb ) )
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.AntiAlias;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.DrawImage( orig, 0, 0, width, height );
                MemoryStream ms = new MemoryStream( );
                thumb.Save( ms, ImageFormat.Jpeg );
                ms.Seek( 0L, SeekOrigin.Begin );
                return ms;
            }
        }
    }
}