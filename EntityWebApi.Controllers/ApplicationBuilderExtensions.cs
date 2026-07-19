using EntityWebApi.Controllers.Interfaces;
using EntityWebApi.Controllers.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace EntityWebApi.Controllers
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures EntityWebApi.
        /// </summary>
        public static IApplicationBuilder UseEntityWebApi(this IApplicationBuilder applicationBuilder, Action<IControllerConfiguration> configure)
        {
            var applicationPartManager = applicationBuilder.ApplicationServices.GetRequiredService<ApplicationPartManager>();
            var options = new FeatureProviderOptions();
            configure(options);
            applicationPartManager.FeatureProviders.Add(new FeatureProvider(options));
            return applicationBuilder;
        }
    }
}