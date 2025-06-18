using Moq;
using System.Linq.Expressions;
using TechChallenge.Region.Domain.Cache;
using TechChallenge.Region.Domain.Region.Entity;
using TechChallenge.Region.Domain.Region.Exception;
using TechChallenge.Region.Domain.Region.Repository;
using TechChallenge.Region.Domain.Region.Service;

namespace TechChallenge.Region.Tests.Domain.Region.Service
{
    public class RegionServiceTest
    {
        private readonly Mock<IRegionRepository> _regionRepositoryMock;
        private readonly Mock<ICacheRepository> _cacheRepositoryMock;
        private readonly RegionService _regionServiceMock;

        public RegionServiceTest()
        {
            _regionRepositoryMock = new Mock<IRegionRepository>();
            _cacheRepositoryMock = new Mock<ICacheRepository>();
            _regionServiceMock = new RegionService(_regionRepositoryMock.Object, _cacheRepositoryMock.Object);
        }

        [Fact(DisplayName = "Should Create A Region")]
        public async Task ShouldCreateARegion()
        {
            var regionMock = new RegionEntity("Test", "11");

            await _regionServiceMock.CreateAsync(regionMock);

            _regionRepositoryMock.Verify(rr => rr.AddAsync(It.IsAny<RegionEntity>()), Times.Once);
        }


        [Fact(DisplayName = "Should Create Throw Exception When Ddd Already Exist")]
        public async Task ShouldCreateThrowExceptionWhenDddAlreadyExist()
        {
            var regionMock = new RegionEntity("Test", "11");

            _regionRepositoryMock.Setup(cr => cr.GetByDddAsync(It.IsAny<string>())).ReturnsAsync((regionMock));

            await Assert.ThrowsAsync<RegionAlreadyExistsException>(() => _regionServiceMock.CreateAsync(regionMock));

            _regionRepositoryMock.Verify(rr => rr.AddAsync(It.IsAny<RegionEntity>()), Times.Never);
            _regionRepositoryMock.Verify(rr => rr.GetByDddAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Should Delete A Region")]
        public async Task ShouldDeleteARegion()
        {
            var idMock = Guid.NewGuid();
            var regionMock = new RegionEntity("Test", "11");

            _regionRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((regionMock));

            await _regionServiceMock.DeleteByIdAsync(idMock);

            _regionRepositoryMock.Verify(rr => rr.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _regionRepositoryMock.Verify(rr => rr.UpdateAsync(It.IsAny<RegionEntity>()), Times.Once);
        }

        [Fact(DisplayName = "Should Delete Throw Exception When Region Does Not Exist")]
        public async Task ShouldDeleteThrowExceptionWhenRegionDoesNotExist()
        {
            var idMock = Guid.NewGuid();
            var regionMock = new RegionEntity("Test", "11");

            _regionRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((RegionEntity)null);

            await Assert.ThrowsAsync<RegionNotFoundException>(() => _regionServiceMock.DeleteByIdAsync(idMock));

            _regionRepositoryMock.Verify(rr => rr.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _regionRepositoryMock.Verify(rr => rr.UpdateAsync(It.IsAny<RegionEntity>()), Times.Never);
        }

        [Fact(DisplayName = "Should Update A Region")]
        public async Task ShouldUpdateARegion()
        {
            var regionMock = new RegionEntity("Test", "11");

            _regionRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(regionMock);

            await _regionServiceMock.UpdateAsync(regionMock);

            _regionRepositoryMock.Verify(rr => rr.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _regionRepositoryMock.Verify(rr => rr.UpdateAsync(It.IsAny<RegionEntity>()), Times.Once);
        }

        [Fact(DisplayName = "Should Update Throw Exception When Region Does Not Exist")]
        public async Task ShouldUpdateThrowExceptionWhenRegionDoesNotExist()
        {
            var regionMock = new RegionEntity("Test", "11");

            _regionRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((RegionEntity)null);

            await Assert.ThrowsAsync<RegionNotFoundException>(() => _regionServiceMock.UpdateAsync(regionMock));

            _regionRepositoryMock.Verify(rr => rr.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _regionRepositoryMock.Verify(rr => rr.UpdateAsync(It.IsAny<RegionEntity>()), Times.Never);
        }

        [Fact(DisplayName = "Should GetById Return Region")]
        public async Task ShouldGetByIdReturnRegion()
        {
            var regionMock = new RegionEntity("Test", "11");

            _regionRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(regionMock);

            var result = await _regionServiceMock.GetByIdAsync(Guid.NewGuid());

            _regionRepositoryMock.Verify(rr => rr.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(result);
        }


        [Fact(DisplayName = "Should Get By Id Cached Return Region")]
        public async Task ShouldGetByIdCachedReturnRegion()
        {
            var regionMock = new RegionEntity("Test", "11");
            _cacheRepositoryMock.Setup(c => c.GetAsync<RegionEntity>(It.IsAny<string>(), It.IsAny<Func<Task<RegionEntity>>>()))
                             .ReturnsAsync(regionMock);

            var result = await _regionServiceMock.GetByIdWithCacheAsync(Guid.NewGuid());

            _cacheRepositoryMock.Verify(c => c.GetAsync<RegionEntity>(It.IsAny<string>(), It.IsAny<Func<Task<RegionEntity>>>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Should GetById Return Exception When Region Does Not Exist")]
        public async Task ShouldGetByIdReturnExceptionWhenRegionDoesNotExist()
        {
            _regionRepositoryMock.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((RegionEntity)null);

            await Assert.ThrowsAsync<RegionNotFoundException>(() => _regionServiceMock.GetByIdAsync(Guid.NewGuid()));

            _regionRepositoryMock.Verify(rr => rr.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Should Get By Id Cached Return Exception When Region Does Not Exist")]
        public async Task ShouldGetByIdCachedReturnExceptionWhenRegionDoesNotExist()
        {

            _cacheRepositoryMock.Setup(c => c.GetAsync<RegionEntity>(It.IsAny<string>(), It.IsAny<Func<Task<RegionEntity>>>()))
                                .ReturnsAsync((RegionEntity)null);

            await Assert.ThrowsAsync<RegionNotFoundException>(() => _regionServiceMock.GetByIdWithCacheAsync(Guid.NewGuid()));

            _cacheRepositoryMock.Verify(c => c.GetAsync<RegionEntity>(It.IsAny<string>(), It.IsAny<Func<Task<RegionEntity>>>()), Times.Once);
        }

        [Fact(DisplayName = "Should GetByDdd Return Region")]
        public async Task ShouldGetByDddReturnRegion()
        {
            var mockDdd = "11";
            var regionMock = new RegionEntity("Test", mockDdd);

            _regionRepositoryMock.Setup(cr => cr.GetByDddAsync(It.IsAny<string>())).ReturnsAsync(regionMock);

            var result = await _regionServiceMock.GetByDdd(mockDdd);

            _regionRepositoryMock.Verify(rr => rr.GetByDddAsync(It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
        }

        //[Fact(DisplayName = "Should Get By Ddd With Contacts Return Region")]
        //public async Task ShouldGetByDddWithContactsReturnRegion()
        //{
        //    var mockDdd = "11";
        //    var regionMock = new RegionEntity("Test", mockDdd);
        //    regionMock.Contacts = new List<ContactEntity>();
        //    regionMock.Contacts.Add(new ContactEntity("Name Mock", "01234567", "mail@mock.com", Guid.NewGuid()));

        //    _regionRepositoryMock.Setup(cr => cr.GetByDddWithContactsAsync(It.IsAny<string>())).ReturnsAsync(regionMock);

        //    var result = await _regionServiceMock.GetByDddWithContacts(mockDdd);

        //    _regionRepositoryMock.Verify(rr => rr.GetByDddWithContactsAsync(It.IsAny<string>()), Times.Once);
        //    Assert.NotNull(result);
        //}

        [Fact(DisplayName = "Should Get All Paged Region")]
        public async Task ShouldGetAllPagedRegion()
        {
            var regionMock = new List<RegionEntity> { new RegionEntity("Test", "11") };

            _regionRepositoryMock.Setup(rr => rr.GetAllPagedAsync(
                                           It.IsAny<Expression<Func<RegionEntity, bool>>>(),
                                           It.IsAny<int>(),
                                           It.IsAny<int>(),
                                           It.IsAny<Expression<Func<RegionEntity, dynamic>>>()
                                       )).ReturnsAsync(regionMock);

            var result = await _regionServiceMock.GetAllPagedAsync(5, 1);

            _regionRepositoryMock.Verify(rr => rr.GetAllPagedAsync(
                                           It.IsAny<Expression<Func<RegionEntity, bool>>>(),
                                           It.IsAny<int>(),
                                           It.IsAny<int>(),
                                           It.IsAny<Expression<Func<RegionEntity, dynamic>>>()
                                       ), Times.Once);

            Assert.NotNull(result);
        }
    }
}
