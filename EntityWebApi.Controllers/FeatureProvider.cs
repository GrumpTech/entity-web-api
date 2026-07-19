using EntityWebApi.Controllers.Internal;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace EntityWebApi.Controllers
{
    public class FeatureProvider(FeatureProviderOptions featureProviderOptions) : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly FeatureProviderOptions FeatureProviderOptions = featureProviderOptions;

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var controllers = FeatureProviderOptions.Controllers.Distinct();
            foreach (var controller in controllers)
            {
                feature.Controllers.Add(controller.GetTypeInfo());
            }
        }
    }
}