using AutoFixture;
using AutoMapper;
using TechChallenge.Region.Api.Controllers.Region.Dto;
using TechChallenge.Region.Domain.Region.Entity;

namespace TechChallenge.Region.Tests.Domain.Region.Entity
{
    public class RegionEntityTest
    {
        private readonly IMapper _mapper;
        public RegionEntityTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegionCreateDto, RegionEntity>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Should Create Entity Region With Exactly Props")]
        public void ShouldCreateEntityRegionWithExactlyProps()
        {
            var regionDto = new Fixture().Create<RegionCreateDto>();

            var entity = _mapper.Map<RegionEntity>(regionDto);

            Assert.IsType<Guid>(entity.Id);
            Assert.Equal(regionDto.Name, entity.Name);
            Assert.Equal(regionDto.Ddd, entity.Ddd);
            Assert.False(entity.IsDeleted);
        }
    }
}
