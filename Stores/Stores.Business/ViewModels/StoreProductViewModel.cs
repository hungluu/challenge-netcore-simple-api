using Stores.Business.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace Stores.Business.ViewModels
{
    public class StoreProductViewModel: ViewModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string ProductType { get; set; }

        [Required]
        public long PackageId { get; set; }
    }
}
