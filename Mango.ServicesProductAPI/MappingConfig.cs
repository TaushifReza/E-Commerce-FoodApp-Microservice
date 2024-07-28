using AutoMapper;
using Mango.ServicesProductAPI.Models;
using Mango.ServicesProductAPI.Models.Dto;

namespace Mango.ServicesProductAPI
{
    public class MappingConfig : Profile
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
