using EntityWebApi.Core.Enums;
using System.Reflection;

namespace EntityWebApi.Dtos.Internal.Dtos
{
    public class PropertyConfiguration
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public IEnumerable<CustomAttributeData>? Attributes { get; private set; }
        public bool Optional { get; private set; } = false;
        public PropertyAccess Access { get; private set; } = PropertyAccess.ReadWrite;

        public PropertyConfiguration(string name, Type type)
        {
            Name = name;
            Type = type;
        }
        public PropertyConfiguration(string name, PropertyInfo propertyInfo)
        {
            Name = name;
            Type = propertyInfo.PropertyType;
            Attributes = propertyInfo.GetCustomAttributesData();
        }

        public PropertyConfiguration WithAttributes(IEnumerable<CustomAttributeData>? attributes)
        {
            Attributes = attributes;
            return this;
        }
        public PropertyConfiguration MakeOptional(bool value)
        {
            Optional = value;
            return this;
        }
        public PropertyConfiguration SetAccess(PropertyAccess access)
        {
            Access = access;
            return this;
        }
    }
}
