using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAPIBasic.Handlers;

namespace WebAPIBasic
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Registering the new DelegatingHandler in the config
            // config.MessageHandlers.Add(new FullPipelineTimerHandler());
            // config.MessageHandlers.Add(new ApiKeyHeaderHandler());
            // config.MessageHandlers.Add(new RemoveBadHeadersHandler());
            config.MessageHandlers.Add(new ForwardedHeadersHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
