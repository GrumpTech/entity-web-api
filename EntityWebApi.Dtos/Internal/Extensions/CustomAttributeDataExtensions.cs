using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;

namespace EntityWebApi.Dtos.Internal.Extensions
{
    public static class CustomAttributeDataExtensions
    {
        public static CustomAttributeBuilder? ToBuilder(this CustomAttributeData attribute)
        {
            var constructorArgumentTypes = attribute.ConstructorArguments
                .Select(a => a.ArgumentType).ToArray();
            var constructorArgumentValues = attribute.ConstructorArguments
                .Select(ConvertConstructorArgument).ToArray();
            var constructorInfo = attribute.AttributeType.GetConstructor(constructorArgumentTypes);

            var propertyArguments = new List<PropertyInfo>();
            var propertyArgumentValues = new List<object>();
            var fieldArguments = new List<FieldInfo>();
            var fieldArgumentValues = new List<object>();
            foreach (var argument in attribute.NamedArguments)
            {
                switch (argument.MemberInfo)
                {
                    case FieldInfo:
                        fieldArguments.Add((FieldInfo)argument.MemberInfo);
                        fieldArgumentValues.Add(argument.TypedValue.Value!);
                        break;
                    case PropertyInfo:
                        propertyArguments.Add((PropertyInfo)argument.MemberInfo);
                        propertyArgumentValues.Add(argument.TypedValue.Value!);
                        break;
                }
            }
            if (constructorInfo != null)
            {
                var customAttributeBuilder = new CustomAttributeBuilder(
                  constructorInfo!,
                  constructorArgumentValues,
                  propertyArguments.ToArray(),
                  propertyArgumentValues.ToArray(),
                  fieldArguments.ToArray(),
                  fieldArgumentValues.ToArray()
                );
                return customAttributeBuilder;
            }
            return null;
        }


        private static object? ConvertConstructorArgument(CustomAttributeTypedArgument argument)
        {
            switch (argument.Value)
            {
                case ReadOnlyCollection<CustomAttributeTypedArgument>:
                    var collection = (ReadOnlyCollection<CustomAttributeTypedArgument>)argument.Value;
                    return collection.Select(v => v.Value).ToArray();
                default:
                    return argument.Value;
            }
        }
    }
}