using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stores.API.Infrastructure;
using Stores.Business.Services;
using Stores.Business.ViewModels;

namespace Stores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : BaseController
    {
        private readonly IStorePackageService PackageService;

        public PackagesController(IStorePackageService packageService)
        {
            PackageService = packageService;
        }

        // GET api/packages
        [HttpGet]
        public async Task<ObjectResult> Get()
        {
            var packages = await PackageService.GetPackages();
            var errors = new string[0];

            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    errors = GetModelErrors()
                });
            } else
            {
                return StatusCode(200, new
                {
                    data = packages
                });
            }
        }

        // POST api/packages
        [HttpPost]
        public async Task<ObjectResult> Post([FromBody] StorePackageViewModel package)
        {
            var createdPackage = await PackageService.CreatePackage(package);

            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    errors = GetModelErrors()
                });
            }
            else
            {
                return StatusCode(200, new
                {
                    data = createdPackage
                });
            }
        }

        // PUT api/packages/5
        [HttpPut("{id}")]
        public async Task<ObjectResult> Put(long id, [FromBody] StorePackageViewModel package)
        {
            var updatedPackage = await PackageService.UpdatePackage(id, package);

            if (!ModelState.IsValid)
            {
                return StatusCode(400, new
                {
                    errors = GetModelErrors()
                });
            }
            else
            {
                return StatusCode(200, new
                {
                    data = updatedPackage
                });
            }
        }
    }
}
