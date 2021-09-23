using System;
using System.Linq;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Realtorist.Extensions.Base;

namespace Realtorist.Web.Extensions
{
    public class MvcExtension : IConfigureServicesExtension, IConfigureApplicationExtension
    {
        public int Priority => int.MaxValue;

        public void ConfigureServices(IServiceCollection services, IServiceProvider serviceProvider)
        {

            serviceProvider.GetService<IWebHostEnvironment>().WebRootFileProvider = this.CreateCompositeFileProvider(serviceProvider);

            var mvcBuilder = services.AddMvc();

            foreach (var assembly in ExtensionManager.Assemblies)
                mvcBuilder.AddApplicationPart(assembly);
        }

        public void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseRouting();
        }

        private IFileProvider CreateCompositeFileProvider(IServiceProvider serviceProvider)
        {
            var fileProviders = new IFileProvider[] {
                serviceProvider.GetService<IWebHostEnvironment>().WebRootFileProvider
            };

            return new CompositeFileProvider(
              fileProviders.Concat(
                ExtensionManager.Assemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
              )
            );
        }
    }
}