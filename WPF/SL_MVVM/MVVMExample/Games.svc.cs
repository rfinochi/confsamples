using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using MVVM.Data;

namespace MVVMExample
{
  public class Games : DataService<GamesEntities>
  {
    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(IDataServiceConfiguration config)
    {
      config.SetEntitySetAccessRule("Games", EntitySetRights.All);
      config.SetEntitySetAccessRule("Suppliers", EntitySetRights.AllRead);

      config.UseVerboseErrors = true;
   }
  }
}
