using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Stores.API.Infrastructure
{
    public abstract class BaseController: ControllerBase
    {
        protected List<ModelErrorCollection> GetModelErrors ()
        {
            return ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
        }
    }
}
