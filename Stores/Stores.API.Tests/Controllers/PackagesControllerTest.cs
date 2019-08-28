using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Stores.API.Controllers;
using Stores.Business.Services;
using Stores.Domain.Models;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Stores.Business.ViewModels;
using System.Linq;

namespace Tests.Controllers
{
    public class PackagesControllerTest
    {
        private PackagesController Controller;
        private IEnumerable<StorePackageViewModel> FakedPackages;
        private Mock<IStorePackageService> PackageServiceMock;

        public PackagesControllerTest()
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
            FakedPackages = new StorePackageViewModel[]
            {
                new StorePackageViewModel()
                {
                    Name = "PackageTest1"
                },
                new StorePackageViewModel()
                {
                    Name = "PackageTest2"
                }
            };

            PackageServiceMock = new Mock<IStorePackageService>();
            
            PackageServiceMock.Setup(m => m.GetPackages()).Returns(Task.FromResult(FakedPackages));
            PackageServiceMock.Setup(m => m.CreatePackage(It.IsAny<StorePackageViewModel>())).Returns<StorePackageViewModel>(p => Task.FromResult(p));
            PackageServiceMock.Setup(m => m.UpdatePackage(It.IsAny<long>(), It.IsAny<StorePackageViewModel>())).Returns<long, StorePackageViewModel>((i, p) => Task.FromResult(new StorePackageViewModel() {
                Id = i,
                Name = p.Name,
                Products = p.Products
            }));

            Controller = new PackagesController(PackageServiceMock.Object);
        }

        [Test]
        public async Task TestCanGetAllPackages()
        {
            ObjectResult response = await Controller.Get();

            var expectedResponse = new ObjectResult(new
            {
                data = FakedPackages
            })
            {
                StatusCode = 200
            };

            PackageServiceMock.Verify(m => m.GetPackages(), Times.Once());
            Assert.AreEqual(response.StatusCode, expectedResponse.StatusCode);
            response.Value.Should().BeEquivalentTo(expectedResponse.Value);
        }

        [Test]
        public async Task TestCanCreatePackage()
        {
            var package = new StorePackageViewModel()
            {
                Name = "PackageTest3",
                Products = new StoreProductViewModel[]
                {
                    new StoreProductViewModel()
                    {
                        Name = "ProductTest1"
                    }
                }
            };
            ObjectResult response = await Controller.Post(package);

            var expectedResponse = new ObjectResult(new
            {
                data = package
            })
            {
                StatusCode = 200
            };

            PackageServiceMock.Verify(m => m.CreatePackage(package), Times.Once());
            Assert.AreEqual(response.StatusCode, expectedResponse.StatusCode);
            response.Value.Should().BeEquivalentTo(expectedResponse.Value);
        }

        [Test]
        public async Task TestCanUpdatePackage()
        {
            var packageId = 1;
            var package = new StorePackageViewModel()
            {
                Name = "PackageTest3",
                Products = new StoreProductViewModel[]
                {
                    new StoreProductViewModel()
                    {
                        Name = "ProductTest1"
                    }
                }
            };
            ObjectResult response = await Controller.Put(packageId, package);

            var expectedPackage = new StorePackageViewModel()
            {
                Id = packageId,
                Name = package.Name,
                Products = package.Products
            };
            var expectedResponse = new ObjectResult(new
            {
                data = expectedPackage
            })
            {
                StatusCode = 200
            };

            PackageServiceMock.Verify(m => m.UpdatePackage(packageId, package), Times.Once());
            Assert.AreEqual(response.StatusCode, expectedResponse.StatusCode);
            response.Value.Should().BeEquivalentTo(expectedResponse.Value);
        }
    }
}