using System.Reflection;

namespace EntityWebApi.MinimalApi.Internal
{
    public class HandlerConfiguration
    {
        public Assembly[] Assemblies { get; }

        public HandlerConfiguration(Assembly[] assemblies)
        {
            Assemblies = assemblies;
        }
    }
}
