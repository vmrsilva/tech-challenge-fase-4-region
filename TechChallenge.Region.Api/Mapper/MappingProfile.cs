using AutoMapper;
using TechChallange.Region.Api.Controllers.Region.Dto;
using TechChallange.Region.Domain.Region.Entity;

namespace TechChallange.Region.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegionCreateDto, RegionEntity>();
            CreateMap<RegionEntity, RegionResponseDto>();
            CreateMap<RegionEntity, RegionWithContactsResponseDto>();
            CreateMap<RegionUpdateDto, RegionEntity>();
        }
    }
}
