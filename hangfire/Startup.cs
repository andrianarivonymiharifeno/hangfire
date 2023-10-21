using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using System.Collections.Generic;
using System.Diagnostics;
using Hangfire.Dashboard;

[assembly: OwinStartup(typeof(hangfire.Startup))]

namespace hangfire
{
    public class Startup
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage("Server=FENO-PC;Database=HangFire;User Id=sa;Password=sql2019;TrustServerCertificate=True;");


            yield return new BackgroundJobServer();
        }
        public void Configuration(IAppBuilder app)
        {
            // Pour plus d'informations sur la configuration de votre application, visitez https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            // Let's also create a sample background job
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });
        }
    }
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // In case you need an OWIN context, use the next line, `OwinContext` class
            // is the part of the `Microsoft.Owin` package.
            var owinContext = new OwinContext(context.GetOwinEnvironment());

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return owinContext.Authentication.User.Identity.IsAuthenticated;
        }
    }
}
