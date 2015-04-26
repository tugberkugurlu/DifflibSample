using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;

namespace DifflibSample
{
    public class Startup
    {
        private readonly IApplicationEnvironment _env;

        public Startup(IApplicationEnvironment env)
        {
            _env = env;
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IFileProvider>(_ => new PhysicalFileProvider(_env.ApplicationBasePath));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{diff=diff1}");
            });
        }
    }
}
