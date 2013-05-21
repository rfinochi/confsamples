using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using MovieIndex.Validators;

namespace MovieIndex
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start( )
        {
            SignalRConfig.RegisterConnections( );
            AutofacConfig.RegisterDependencies( );
            
            AreaRegistration.RegisterAllAreas( );

            WebApiConfig.ConfigureApis( GlobalConfiguration.Configuration );
            WebApiConfig.Register( GlobalConfiguration.Configuration );
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );
           
            DataAnnotationsModelValidatorProvider.RegisterAdapter( typeof( RatingByGenreAttribute ), typeof( RatingByGenreValidator ) );
        }
    }
}