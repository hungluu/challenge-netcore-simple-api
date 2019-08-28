using Microsoft.AspNetCore.Mvc;
using Stores.API.Infrastructure;

namespace Stores.API.Controllers
{
    public class DefaultController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}