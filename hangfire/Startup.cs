using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using System.Collections.Generic;
using System.Diagnostics;

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
                .UseSqlServerStorage("Data Source=FENO-PC;Initial Catalog=Hangfire;User ID=sa;Password=sql2019");

            yield return new BackgroundJobServer();
        }
        public void Configuration(IAppBuilder app)
        {
            // Pour plus d'informations sur la configuration de votre application, visitez https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            // Let's also create a sample background job
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
        }
    }
}
