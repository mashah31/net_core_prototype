using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ABC.NetCore.Models;
using Microsoft.Extensions.Options;

namespace ABC.NetCore.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ValidateAPIModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check and confirm if action arguments are not null
            if (filterContext.ActionArguments.Any(x => x.Value == null))
            {
                filterContext.ModelState.AddModelError("NullValue", "Arguments must have to be provided. Arguments cannot be null. Check get response for format of API.");
            }

            // Check if provided model state is valid or not?
            if (!filterContext.ModelState.IsValid)
            {
                filterContext.Result = new BadRequestObjectResult(new ApiError(filterContext.ModelState));
            }
        }
    }
}
