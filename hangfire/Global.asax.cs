using Hangfire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace hangfire
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            Hangfire.GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("Server=FENO-PC;Database=HangFire;User Id=sa;Password=sql2019;TrustServerCertificate=True;");

            yield return new BackgroundJobServer();
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HangfireAspNet.Use(GetHangfireServers);

            // Let's also create a sample background job
            BackgroundJob.Enqueue(() => Debug.WriteLine("Hello world from Hangfire!"));
        }
    }
}
