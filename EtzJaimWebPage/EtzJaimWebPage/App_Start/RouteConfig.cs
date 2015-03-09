using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EtzJaimWebPage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "aprobarCita",
               url: "{controller}/{action}/{cita_id}",
               defaults: new { controller = "Citas", action = "Aprobar", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "rechazarCita",
              url: "{controller}/{action}/{cita_id}",
              defaults: new { controller = "Citas", action = "Rechazar", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "Pull",
             url: "{controller}/{action}/{conv_id}",
             defaults: new { controller = "Conversaciones", action = "Pull", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "Send",
             url: "{controller}/{action}/{conv_id}/{message}/{username}",
             defaults: new { controller = "Conversaciones", action = "Send", id = UrlParameter.Optional }
         );
            routes.MapRoute(
            name: "Ajax",
            url: "{controller}/{action}/{user}",
            defaults: new { controller = "Usuarios", action = "Ajax", id = UrlParameter.Optional }
        );
            routes.MapRoute(
           name: "EditProduct",
           url: "{controller}/{action}/{pro_id}/{pro_nombre}/{pro_precio}/{pro_estado}",
           defaults: new { controller = "Productos", action = "EditProduct", id = UrlParameter.Optional }
       );
        }
    }
}
