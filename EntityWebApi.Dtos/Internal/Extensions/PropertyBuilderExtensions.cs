using System.Reflection;
using System.Reflection.Emit;

namespace EntityWebApi.Dtos.Internal.Extensions
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder AddAttributes(this PropertyBuilder propertyBuilder, IEnumerable<CustomAttributeData> attributes)
        {
            foreach (var attribute in attributes)
            {
                propertyBuilder.AddAttribute(attribute);
            }
            return propertyBuilder;
        }

        public static PropertyBuilder AddAttribute(this PropertyBuilder propertyBuilder, CustomAttributeData attribute)
        {
            var attributeBuilder = attribute.ToBuilder();
            if (attributeBuilder != null)
            {
                propertyBuilder.SetCustomAttribute(attributeBuilder);
            }
            return propertyBuilder;
        }
    }
}