using AutoMapper;
using TechChallenge.Region.Api.Controllers.Region.Dto;
using TechChallenge.Region.Domain.Region.Entity;

namespace TechChallenge.Region.Api.Mapper
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
