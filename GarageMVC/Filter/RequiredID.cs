using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageMVC.Filter
{
    public class RequiredID : ActionFilterAttribute
    {
        private readonly string name;

        public RequiredID(string name)
        {
            this.name = name;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ActionArguments.TryGetValue(name, out object x))
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
