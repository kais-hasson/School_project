using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;

namespace School_project.Configuration
{
    public class WepApiConfiguration
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "StudentRout",
                routeTemplate: "api/Page_Registration"
               );
           /* config.Routes.MapHttpRoute(
                name: "SongsRout",
                routeTemplate:"api/Songs/{str},{id}",
                defaults:new {str=RouteParameter.Optional},
                new { id = RouteParameter.Optional }
                );*/
        }
    }
}