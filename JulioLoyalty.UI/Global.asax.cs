using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace JulioLoyalty.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalFilters.Filters.Add(new MVCExceptionFilter());
        }
    }

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MVCExceptionFilter : System.Web.Mvc.IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            Trace.TraceError(filterContext.Exception.ToString());
        }
    }   
    
    public class APIExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Models.Audit audit = new Models.Audit();
            
            if (actionExecutedContext.Exception.InnerException == null)
                audit.Messsage = actionExecutedContext.Exception.Message;
            else
                audit.InnerExceptionMessage = actionExecutedContext.Exception.InnerException.Message;
       
            var request = HttpContext.Current.Request;
            var _user = actionExecutedContext.ActionContext.RequestContext.Principal.Identity;
            
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var parameter in actionExecutedContext.ActionContext.ActionArguments)
            {
                parameters.Add(parameter.Key, Newtonsoft.Json.JsonConvert.SerializeObject(parameter.Value));
            }

            var ex = actionExecutedContext.Exception;

            audit.NumException = null; //ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            audit.ExceptionName = ex.GetType().Name.ToString();
            audit.ExceptionType = ex.GetType().ToString();    
            audit.Id = (request.IsAuthenticated) ? Guid.Parse(_user.GetUserId()) : Guid.NewGuid();
            audit.Usuario = (request.IsAuthenticated) ? _user.Name : "Anonymous";
            audit.Direccion_ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            audit.Area_acceso = request.RawUrl;
            audit.FechaYhora = DateTime.Now;
            audit.Parametros = parameters;
            audit.Navegador = request.Browser.Browser;
            audit.UAgente = request.UserAgent;
            audit.Detalle = string.Format("Browser: {0}, Type: {1}, Version: {2}, MVersion: {3}", request.Browser.Browser, request.Browser.Type, request.Browser.Version, request.Browser.MajorVersion);

            // save event log
            audit.SaveException();

            //We can log this exception message to the file or database.  
            var response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new System.Net.Http.StringContent("An unhandled exception was thrown by service."),  
                    ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
            };
            actionExecutedContext.Response = response;  
        }
    }
}
