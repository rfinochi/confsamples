using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net.Mime;
using FrasesDeTodos.Models;
using FrasesDeTodos.MVCExtension.ModelBinders;
using FrasesDeTodos.MVCExtension.CustomViewEngine;
using FrasesDeTodos.MVCExtension.ExceptionFilters;
using FrasesDeTodos.MVCExtension.ValueProviders;
using FrasesDeTodos.MVCExtension.Filters;

namespace FrasesDeTodos
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private void RegisterAuthorCookie()
        {
            var autorCookie = new HttpCookie("autorCookie");
            autorCookie.Values.Add("deQuien", "Mariano");

            HttpContext.Current.Request.Cookies.Add(autorCookie);
        }

        private void RegisterValueProviders()
        {
            ValueProviderFactories.Factories.Add(new CookieValueProviderFactory());
        }

        private void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(Frase), new FraseModelBinder());
        }

        private void RegisterCustomViewEngines()
        {
            ViewEngines.Engines.Add(new CustomListsViewEngine());
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CacheInvalidationFilter());
            filters.Add(new PlainTextExceptionFilter());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Custom",
                "{deQuien}",
                //new { controller = "Frases", action = "Index", deQuien = UrlParameter.Optional }
                new { controller = "Frases", action = "Index", deQuien = "Krako" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            //RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterModelBinders();
            //RegisterCustomViewEngines();
            //RegisterValueProviders();
        }

        protected void Application_BeginRequest()
        {
            RegisterAuthorCookie();
        }
    }
}