using AutoMapper;

namespace EntityWebApi.AutoMapper.Interfaces
{
    public interface IMapperStore
    {
        public void AddMapper(string name, IMapper mapper);
        public IMapper GetMapper(string name);
    }
}
