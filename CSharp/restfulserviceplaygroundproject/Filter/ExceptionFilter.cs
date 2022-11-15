using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using restfulserviceplaygroundproject.Infrastructure;

namespace restfulserviceplaygroundproject.Filter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var req = context.HttpContext.Request;
            var routeValue = context.HttpContext.GetRouteData().Values;
            var method = req.Method;
            var host = req.Host.ToString();
            var path = req.Path.ToString();
            var query = req.Query.ToString();
            var body = context.HttpContext.Request.Body ?? null;
            var form = req.Form ?? null;
            context.Result = new JsonResult(Result.Fail<object>(new {
                RouteValue = routeValue,
                Method = method,
                Host = host,
                Path = path,
                Query = query,
                Body = body,
                Form = form,
            }, context.Exception.Message));
            base.OnException(context);
        }
    }
}