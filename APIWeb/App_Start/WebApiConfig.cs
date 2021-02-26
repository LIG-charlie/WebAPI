using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace APIWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{ext}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, extensions = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.AddUriPathExtensionMapping("json", "application/json");
            config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", "text/xml");
        }
    }
}
