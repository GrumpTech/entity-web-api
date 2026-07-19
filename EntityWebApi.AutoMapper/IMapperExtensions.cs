using AutoMapper;
using System;

namespace EntityWebApi.AutoMapper
{
    public static class IMapperExtensions
    {
        public static T MapNotNull<T>(this IMapper mapper, object source)
        {
            return mapper.Map<T>(source) ?? throw new ArgumentException("Unexpected null result automapper.");
        }
    }
}