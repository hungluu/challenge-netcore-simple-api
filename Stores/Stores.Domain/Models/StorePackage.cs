using Stores.Domain.SeedWork;
using System.Collections.Generic;

namespace Stores.Domain.Models
{
    public class StorePackage : Entity
    {
        public string Name { get; set; }

        public virtual IEnumerable<StoreProduct> Products { get; set; }
    }
}
