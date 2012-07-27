using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelloWorld
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                "Hola",
                "Hola/{nombre}",
                new
                {
                    controller = "Foo",
                    action = "Hello",
                    nombre = "Rodolfo"
                }
            );

            //routes.MapRoute(
            //    "Default",
            //    "{controller}/{action}/{nombre}",
            //    new
            //    {
            //        controller = "Foo",
            //        action = "Hello",
            //        nombre = "Amigos"
            //    });

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = ""
                }
            );
        }

        protected void Application_Start( )
        {
            RegisterRoutes( RouteTable.Routes );
        }
    }
}