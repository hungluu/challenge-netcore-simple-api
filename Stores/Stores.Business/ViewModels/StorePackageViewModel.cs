using Stores.Business.SeedWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stores.Business.ViewModels
{
    public class StorePackageViewModel: ViewModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual IEnumerable<StoreProductViewModel> Products { get; set; }
    }
}
