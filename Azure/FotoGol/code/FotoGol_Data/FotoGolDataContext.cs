using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace FotoGol_Data
{
    public class FotoGolDataContext : TableServiceContext
    {
        public FotoGolDataContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        { }

        public IQueryable<FotoGolEntry> FotoGolEntry
        {
            get
            {
                return this.CreateQuery<FotoGolEntry>("FotoGolEntry");
            }
        }
    }
}