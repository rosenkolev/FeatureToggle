using System.Web.Mvc;
using System.Web.Routing;

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FeatureToggle.Presentation.Web.Startup))]

namespace FeatureToggle.Presentation.Web
{
    /// <summary>
    /// Startup Owin Configuration Class.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Initial Configuration Method.
        /// </summary>
        public static void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WindsorConfig.Setup();
        }
    }
}