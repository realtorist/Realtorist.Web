using System;
using System.Linq;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Realtorist.Extensions.Base.Manager;
using Realtorist.Web.Extensions;
using Realtorist.Web.Helpers;

namespace Realtorist.Web
{
    public class Startup
    {
        private readonly string _extensionsPath;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;

            _extensionsPath = env.ContentRootPath + "/" + configuration["Extensions:Path"];
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IWebHostEnvironment>(CurrentEnvironment);

            services.AddSingleton<IExtensionManager, ExtCoreExtensionManager>();

            services.AddLogging(loggingBuilder =>
            {
                var loggingSection = Configuration.GetSection("Logging");
                loggingBuilder.AddFile(loggingSection);
            });

            var defaultWebRootProvider = CurrentEnvironment.WebRootFileProvider;
            if (defaultWebRootProvider is CompositeFileProvider)
            {
                defaultWebRootProvider = (defaultWebRootProvider as CompositeFileProvider).FileProviders.First(x => x is PhysicalFileProvider);
            }

            if (defaultWebRootProvider is not PhysicalFileProvider)
            {
                throw new InvalidOperationException($"Unsupported default file provider of {defaultWebRootProvider.GetType()}");
            }

            services.AddExtensions(_extensionsPath, true);

            if (CurrentEnvironment.WebRootFileProvider is CompositeFileProvider)
            {
                CurrentEnvironment.WebRootFileProvider = new CompositeWithFallbackFileProvider(
                  CurrentEnvironment.WebRootFileProvider as CompositeFileProvider,
                  defaultWebRootProvider as PhysicalFileProvider);
            }
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExtensions();
        }
    }
}
