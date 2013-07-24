using System.Web.Http;
using System.Web.Http.OData.Query;
using MovieIndex.Formatters;

namespace MovieIndex
{
    public static class WebApiConfig
    {
        public static void ConfigureApis( HttpConfiguration config )
        {
            //config.Formatters.XmlFormatter.UseXmlSerializer = true;
            
            config.Formatters.Add( new MoviesCsvFormatter( ) );
        }

        public static void Register( HttpConfiguration config )
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //var queryAttribute = new QueryableAttribute( )
            //{
            //    AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.Skip,
            //    MaxTop = 100
            //};

            //config.EnableQuerySupport( queryAttribute );

            config.EnableQuerySupport( );
        }
    }
}