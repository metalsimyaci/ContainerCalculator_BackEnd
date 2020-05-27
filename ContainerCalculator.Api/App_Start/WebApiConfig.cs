using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;

namespace ContainerCalculator.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var constraintResolver = new DefaultInlineConstraintResolver() { ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) } };
            config.AddApiVersioning(o => o.ReportApiVersions = true);
            config.MapHttpAttributeRoutes(constraintResolver);
            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);
        }
    }
}
