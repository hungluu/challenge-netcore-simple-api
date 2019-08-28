using Stores.Domain.SeedWork;

namespace Stores.Domain.Models
{
    public class StoreProduct : Entity
    {
        public string Name { get; set; }
        public string ProductType { get; set; }
        public long PackageId { get; set; }

        public virtual StorePackage Package { get; set; }
    }
}
