namespace EntityWebApi.Core
{
    public class TypeStore
    {
        private readonly Dictionary<string, Type> _types = new();

        public Type? Get(string name)
        {
            _types.TryGetValue(name, out var result);
            return result;
        }

        public Type GetRequired(string name)
        {
            _types.TryGetValue(name, out var result);
            return result ?? throw new ArgumentException($"EntityWebApi: No type '{name}' found in type store");
        }

        public void Add(Type type)
        {
            if (!_types.TryAdd(type.Name, type))
            {
                throw new ArgumentException($"EntityWebApi: A type with the same name {type.Name} was added before");
            }
        }
    }
}
