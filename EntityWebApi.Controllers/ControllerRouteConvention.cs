using EntityWebApi.Core;
using EntityWebApi.Core.Attributes;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace EntityWebApi.Controllers
{
    public class ControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var replacements = RouteValueResolver.GetReplacements(controller.ControllerType);
            foreach (var replacement in replacements)
            {
                controller.RouteValues[replacement.Key] = replacement.Value;
            }
            var tagAttribute = controller.ControllerType.GetCustomAttribute<OpenApiTagAttribute>();
            if (tagAttribute != null)
            {
                controller.RouteValues["Controller"] = ReplaceMultiple(tagAttribute.Template, replacements);
            }
        }

        private static string ReplaceMultiple(string value, IDictionary<string, string> replacements)
        {
            foreach (var replacement in replacements)
            {
                value = value.Replace($"[{replacement.Key}]", replacement.Value);
            }
            return value;
        }
    }
}