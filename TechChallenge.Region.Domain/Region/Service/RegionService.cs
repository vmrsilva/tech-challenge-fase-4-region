using TechChallenge.Region.Domain.Cache;
using TechChallenge.Region.Domain.Region.Entity;
using TechChallenge.Region.Domain.Region.Exception;
using TechChallenge.Region.Domain.Region.Repository;

namespace TechChallenge.Region.Domain.Region.Service
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly ICacheRepository _cacheRepository;

        public RegionService(IRegionRepository regionRepository, ICacheRepository cacheRepository)
        {
            _regionRepository = regionRepository;
            _cacheRepository = cacheRepository;
        }

        public async Task CreateAsync(RegionEntity regionEntity)
        {
            var regionDb = await _regionRepository.GetByDddAsync(regionEntity.Ddd).ConfigureAwait(false);

            if (regionDb != null)
                throw new RegionAlreadyExistsException();

            await _regionRepository.AddAsync(regionEntity).ConfigureAwait(false);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var regionDb = await _regionRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (regionDb == null)
                throw new RegionNotFoundException();

            regionDb.MarkAsDeleted();

            await _regionRepository.UpdateAsync(regionDb).ConfigureAwait(false);
        }

        public async Task<IEnumerable<RegionEntity>> GetAllPagedAsync(int pageSize, int page)
        {
            return await _regionRepository.GetAllPagedAsync(r => !r.IsDeleted, pageSize, page, r => r.Name).ConfigureAwait(false);
        }

        public async Task<RegionEntity> GetByDdd(string ddd)
        {
            var region = await _regionRepository.GetByDddAsync(ddd).ConfigureAwait(false);

            if (region == null)
                throw new RegionNotFoundException();

            return region;
        }

        public async Task<RegionEntity> GetByDddWithContacts(string ddd)
        {
            return await _regionRepository.GetByDddWithContactsAsync(ddd).ConfigureAwait(false);
        }

        public async Task<RegionEntity> GetByIdAsync(Guid id)
        {
            var result = await _regionRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (result == null)
                throw new RegionNotFoundException();

            return result;
        }

        public async Task<RegionEntity> GetByIdWithCacheAsync(Guid id)
        {
            var result = await _cacheRepository.GetAsync(id.ToString(), async () => await _regionRepository.GetByIdAsync(id).ConfigureAwait(false));

            if (result == null)
                throw new RegionNotFoundException();

            return result;
        }

        public async Task<int> GetCountAsync()
        {
            return await _regionRepository.GetCountAsync(r => !r.IsDeleted).ConfigureAwait(false);
        }

        public async Task UpdateAsync(RegionEntity regionEntity)
        {
            var regionDb = await GetByIdAsync(regionEntity.Id).ConfigureAwait(false);

            if (regionDb == null)
                throw new RegionNotFoundException();

            regionDb.Name = regionEntity.Name;
            regionDb.Ddd = regionEntity.Ddd;

            await _regionRepository.UpdateAsync(regionDb).ConfigureAwait(false);
        }
    }
}
