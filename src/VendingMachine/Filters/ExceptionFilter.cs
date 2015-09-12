using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using VendingMachineApp.App_Data;

namespace VendingMachineApp.Filters
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        private readonly Type[] _exceptions;

        public ExceptionFilter(params Type[] exceptions)
        {
            _exceptions = exceptions;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            var isAjax = filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var exception = filterContext.Exception;
            var errorMessage = !_exceptions.Contains(exception.GetType()) 
                ? ErrorMessage.UnknownError 
                : ErrorMessage.ResourceManager.GetString(exception.GetType().Name);
            filterContext.ExceptionHandled = true;

            if (isAjax)
            {
                filterContext.Result = new JsonResult { Data = new { Error = true, Message = errorMessage, ErrorType = exception.GetType().Name } };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult("ErrorPage", new RouteValueDictionary { { "message", errorMessage } });
            }
        }
    }
}