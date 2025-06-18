using System.Linq.Expressions;
using TechChallenge.Region.Domain.Base.Repository;
using TechChallenge.Region.Domain.Region.Entity;
using TechChallenge.Region.Domain.Region.Repository;

namespace TechChallenge.Region.Infrastructure.Repository.Region
{
    public class RegionRepository : IRegionRepository
    {
        private readonly IBaseRepository<RegionEntity> _baseRepository;

        public RegionRepository(IBaseRepository<RegionEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task AddAsync(RegionEntity entity)
        {
            await _baseRepository.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<RegionEntity>> GetAllPagedAsync(Expression<Func<RegionEntity, bool>> search, int pageSize, int page, Expression<Func<RegionEntity, dynamic>> orderDesc)
        {
            return await _baseRepository.GetPagedAsync(search, pageSize, page, orderDesc);
        }

        public async Task<RegionEntity> GetByDddAsync(string ddd)
        {
            return await _baseRepository.GetAsync(r => r.Ddd == ddd && !r.IsDeleted).ConfigureAwait(false);
        }

        public async Task<RegionEntity> GetByDddWithContactsAsync(string ddd)
        {
            //return await _baseRepository.GetOneWithIncludeAsync(r => r.Ddd == ddd && !r.IsDeleted, r => r.Contacts).ConfigureAwait(false);
            throw new Exception();
        }

        public async Task<RegionEntity> GetByIdAsync(Guid id)
        {
            return await _baseRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            await _baseRepository.RemoveByIdAsync(id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(RegionEntity entity)
        {
            await _baseRepository.UpdateAsync(entity).ConfigureAwait(false);
        }

        public async Task<int> GetCountAsync(Expression<Func<RegionEntity, bool>> search)
        {
            return await _baseRepository.GetCountAsync(search).ConfigureAwait(false);
        }
    }
}
