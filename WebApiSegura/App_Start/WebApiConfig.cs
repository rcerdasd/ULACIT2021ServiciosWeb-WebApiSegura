using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiSegura.Controllers;

namespace WebApiSegura
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web-API-Konfiguration und -Dienste

            // Web-API-Routen
            config.MapHttpAttributeRoutes();

            config.MessageHandlers.Add(new TokenValidationHandler());


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
