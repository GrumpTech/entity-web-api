namespace EntityWebApi.MinimalApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenericHandlerAttribute : Attribute
    {
        public GenericHandlerAttribute() { }
    }
}