using System.Linq.Expressions;
using TechChallenge.Region.Domain.Region.Entity;

namespace TechChallenge.Region.Domain.Region.Repository
{
    public interface IRegionRepository
    {
        Task AddAsync(RegionEntity entity);
        Task RemoveByIdAsync(Guid id);
        Task UpdateAsync(RegionEntity entity);
        Task<RegionEntity> GetByIdAsync(Guid id);
        Task<RegionEntity> GetByDddAsync(string ddd);
        Task<IEnumerable<RegionEntity>> GetAllPagedAsync(Expression<Func<RegionEntity, bool>> search, int pageSize, int page, Expression<Func<RegionEntity, dynamic>> orderDesc);
        Task<RegionEntity> GetByDddWithContactsAsync(string ddd);
        Task<int> GetCountAsync(Expression<Func<RegionEntity, bool>> search);

    }
}
