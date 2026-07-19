using System.Reflection;
using System.Reflection.Emit;

namespace EntityWebApi.MinimalApi.Internal.BuilderExtensions
{
    public static class MethodBuilderExtensions
    {
        public static IEnumerable<ParameterBuilder> AddParameters(this MethodBuilder methodBuilder, IEnumerable<ParameterInfo> parameters, int index = 1)
        {
            return parameters.Select((p, idx) => methodBuilder.AddParameter(p, index + idx)).ToList();
        }

        public static ParameterBuilder AddParameter(this MethodBuilder methodBuilder, ParameterInfo parameter, int index)
        {
            var parameterBuilder = methodBuilder.DefineParameter(index, parameter.Attributes, parameter.Name);
            return parameterBuilder.AddAttributes(parameter.GetCustomAttributesData() ?? Array.Empty<CustomAttributeData>());
        }

        public static ParameterBuilder AddParameter(this MethodBuilder methodBuilder, string name, ParameterAttributes attribute, int index)
        {
            return methodBuilder.DefineParameter(index, attribute, name);
        }

        public static MethodBuilder AddAttributes(this MethodBuilder methodBuilder, IEnumerable<CustomAttributeData> attributes)
        {
            foreach (var attribute in attributes)
            {
                methodBuilder.AddAttribute(attribute);
            }
            return methodBuilder;
        }

        public static MethodBuilder AddAttribute(this MethodBuilder methodBuilder, CustomAttributeData attribute)
        {
            var attributeBuilder = attribute.ToBuilder();
            if (attributeBuilder != null)
            {
                methodBuilder.SetCustomAttribute(attributeBuilder);
            }
            return methodBuilder;
        }
    }
}