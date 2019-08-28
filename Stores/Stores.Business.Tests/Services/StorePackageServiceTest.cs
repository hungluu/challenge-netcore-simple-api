using NUnit.Framework;
using Moq;
using Stores.Business.Services;
using Stores.Domain.Models;
using Stores.Infrastructure.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Stores.Business.ViewModels;
using AutoMapper;

namespace Tests.Services
{
    public class StorePackageServiceTest
    {
        private IStorePackageService PackageService;
        private Mock<IRepository<StorePackage>> PackageRepositoryMock;
        private Mock<IRepository<StoreProduct>> ProductRepositoryMock;
        private IQueryable<StorePackage> FakedPackages;

        public StorePackageServiceTest ()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<StoreProductViewModel, StoreProduct>().ReverseMap();
                config.CreateMap<StorePackageViewModel, StorePackage>().ReverseMap();
            });
        }

        [SetUp]
        public void Setup()
        {
            PackageRepositoryMock = new Mock<IRepository<StorePackage>>();
            ProductRepositoryMock = new Mock<IRepository<StoreProduct>>();

            FakedPackages = new StorePackage[] {
                new StorePackage()
                {
                    Name = "PackageTest1"
                },
                new StorePackage()
                {
                    Name = "PackageTest2"
                }
            }.AsQueryable();

            PackageRepositoryMock.Setup(m => m.Create(It.IsAny<StorePackage>())).Returns(Task.CompletedTask);
            PackageRepositoryMock.Setup(m => m.Find()).Returns(FakedPackages);
            ProductRepositoryMock.Setup(m => m.Create(It.IsAny<StoreProduct>())).Returns(Task.CompletedTask);
            PackageRepositoryMock.Setup(m => m.Update(It.IsAny<long>(), It.IsAny<StorePackage>())).Returns(Task.CompletedTask);

            PackageService = new StorePackageService(PackageRepositoryMock.Object, ProductRepositoryMock.Object);
        }

        [Test]
        public void ShouldCreatePackageByRepository()
        {
            var package = new StorePackageViewModel();

            PackageService.CreatePackage(package);
            PackageRepositoryMock.Verify(m => m.Create(It.IsAny<StorePackage>()), Times.Once());
        }

        [Test]
        public async Task ShouldReturnCreatedPackage()
        {
            var package = new StorePackageViewModel()
            {
                Name = "PackageTest3"
            };
            var createdPackage = await PackageService.CreatePackage(package);

            Assert.AreEqual(createdPackage.Name, package.Name);
        }

        [Test]
        public async Task ShouldGetPackagesFromRepository()
        {
            var packages = await PackageService.GetPackages();
            string[] getPackageNames = packages.Select(p => p.Name).ToArray();
            string[] repoPackageNames = FakedPackages.Select(p => p.Name).ToArray();

            Assert.AreEqual(getPackageNames, repoPackageNames);
        }

        [Test]
        public async Task ShouldReturnUpdatedPackage()
        {
            var packageId = 1;
            var package = new StorePackageViewModel()
            {
                Name = "PackageTest2"
            };

            var updatedPackage = await PackageService.UpdatePackage(packageId, package);

            Assert.AreEqual(updatedPackage.Id, packageId);
            Assert.AreEqual(updatedPackage.Name, package.Name);
        }
    }
}