using System;
using System.Text;
using System.Web.Mvc;

namespace Web.ViewModels
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
  public sealed class HandleModelStateExceptionAttribute : FilterAttribute, IExceptionFilter
  {

    public void OnException(ExceptionContext filterContext)
    {
      if (filterContext == null)
      {
        throw new ArgumentNullException("filterContext");
      }

      if (!(filterContext.Exception is ModelStateException) || filterContext.ExceptionHandled)
      {
        return;
      }

      filterContext.ExceptionHandled = true;
      var currentResponse = filterContext.HttpContext.Response;
      currentResponse.Clear();
      currentResponse.ContentEncoding = Encoding.UTF8;
      currentResponse.HeaderEncoding = Encoding.UTF8;
      currentResponse.TrySkipIisCustomErrors = true;
      currentResponse.StatusCode = 400;
      filterContext.Result = new ContentResult
      {
        Content = ((ModelStateException) filterContext.Exception).Message,
        ContentEncoding = Encoding.UTF8,
      };
    }
  }
}