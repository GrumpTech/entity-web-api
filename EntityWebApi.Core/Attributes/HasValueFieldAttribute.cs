namespace EntityWebApi.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class HasValueFieldAttribute : Attribute
    {
        public string Name { get; }
        public HasValueFieldAttribute(string name)
        {
            Name = name;
        }
    }
}
