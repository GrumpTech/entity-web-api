using AutoMapper;
using EntityWebApi.AutoMapper.Interfaces;
using System;
using System.Collections.Concurrent;

namespace EntityWebApi.AutoMapper
{
    public class MapperStore : IMapperStore
    {
        private readonly ConcurrentDictionary<string, IMapper> _mappers = new();

        public void AddMapper(string name, IMapper mapper)
        {
            if (!_mappers.TryAdd(name, mapper))
            {
                throw new InvalidOperationException($"EntityWebApi: Mapping '{name}' was already added.");
            }
        }

        public IMapper GetMapper(string name)
        {
            return _mappers.TryGetValue(name, out var result) ? result :
                throw new InvalidOperationException($"EntityWebApi: Mapping '{name}' not added.");
        }
    }
}