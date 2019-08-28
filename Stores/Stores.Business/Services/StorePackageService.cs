using Stores.Domain.Models;
using Stores.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Stores.Business.ViewModels;
using AutoMapper;

namespace Stores.Business.Services
{
    public interface IStorePackageService
    {
        Task<StorePackageViewModel> CreatePackage(StorePackageViewModel package);

        Task<IEnumerable<StorePackageViewModel>> GetPackages();

        Task<StorePackageViewModel> UpdatePackage(long packageId, StorePackageViewModel package);
    }

    public class StorePackageService : IStorePackageService
    {
        private readonly IRepository<StorePackage> PackageRepository;
        private readonly IRepository<StoreProduct> ProductRepository;

        public StorePackageService (IRepository<StorePackage> packageRepository, IRepository<StoreProduct> productRepository)
        {
            PackageRepository = packageRepository;
            ProductRepository = productRepository;
        }

        public async Task<StorePackageViewModel> CreatePackage(StorePackageViewModel package)
        {
            await PackageRepository.Create(Mapper.Map<StorePackage>(package));

            return package;
        }

        public Task<IEnumerable<StorePackageViewModel>> GetPackages()
        {
            return Task.FromResult(PackageRepository.Find().Include(package => package.Products).Select(p => Mapper.Map<StorePackageViewModel>(p)).AsEnumerable());
        }

        public async Task<StorePackageViewModel> UpdatePackage(long packageId, StorePackageViewModel package)
        {
            var updatedPackage = new StorePackageViewModel()
            {
                Id = packageId,
                Name = package.Name,
                Products = package.Products
            };

            // Remove old products
            var oldProducts = ProductRepository.Find().Where(p => p.PackageId == packageId).ToArray();
            
            foreach (StoreProduct oldProduct in oldProducts)
            {
                await ProductRepository.Delete(oldProduct.Id);
            }

            await PackageRepository.Update(packageId, Mapper.Map<StorePackage>(updatedPackage));

            return updatedPackage;
        }
    }
}
