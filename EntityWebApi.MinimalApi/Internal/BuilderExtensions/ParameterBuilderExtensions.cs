using System.Reflection;
using System.Reflection.Emit;

namespace EntityWebApi.MinimalApi.Internal.BuilderExtensions
{
    public static class ParameterBuilderExtensions
    {
        public static ParameterBuilder AddAttributes(this ParameterBuilder parameterBuilder, IEnumerable<CustomAttributeData> attributes)
        {
            foreach (var attribute in attributes)
            {
                parameterBuilder.AddAttribute(attribute);
            }
            return parameterBuilder;
        }

        public static ParameterBuilder AddAttribute(this ParameterBuilder parameterBuilder, CustomAttributeData attribute)
        {
            var attributeBuilder = attribute.ToBuilder();
            if (attributeBuilder != null)
            {
                parameterBuilder.SetCustomAttribute(attributeBuilder);
            }
            return parameterBuilder;
        }
    }
}