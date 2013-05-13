using System.Web.Routing;

namespace MovieIndex
{
    public class SignalRConfig
    {
        public static void RegisterConnections( )
        {
            RouteTable.Routes.MapHubs( );
        }
    }
}