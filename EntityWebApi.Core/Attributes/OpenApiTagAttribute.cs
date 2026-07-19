namespace EntityWebApi.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class OpenApiTagAttribute : Attribute
    {
        public string Template { get; }

        public OpenApiTagAttribute(string template)
        {
            Template = template;
        }
    }
}