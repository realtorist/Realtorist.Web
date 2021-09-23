using System.Linq;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Realtorist.Extensions.Base;

namespace Realtorist.Web.Helpers
{
    /// <summary>
    /// Contains the extension methods of the <see cref="IApplicationBuilder">IApplicationBuilder</see> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Executes the Configure actions from all the extensions. It must be called inside the Configure method
        /// of the web application's Startup class in order to work properly.
        /// </summary>
        /// <param name="applicationBuilder">
        /// The application builder passed to the Configure method of the web application's Startup class.
        /// </param>
        public static void UseExtensions(this IApplicationBuilder applicationBuilder)
        {
            var logger = applicationBuilder.ApplicationServices.GetService<ILoggerFactory>().CreateLogger("Reatorist.Web");

            foreach (var action in ExtensionManager.GetInstances<IConfigureApplicationExtension>().OrderBy(a => a.Priority))
            {
                logger.LogInformation("Executing Configure action '{0}'", action.GetType().FullName);
                action.ConfigureApplication(applicationBuilder, applicationBuilder.ApplicationServices);
            }
        }
    }
}