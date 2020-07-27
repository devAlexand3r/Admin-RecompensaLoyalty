using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Models
{
    public class Audit
    {
        public Guid Id { get; set; }
        public string Usuario { get; set; }
        public string Direccion_ip { get; set; }
        public string Area_acceso { get; set; }
        public DateTime FechaYhora { get; set; }
        public string Navegador { get; set; }
        public string Detalle { get; set; }
        public string UAgente { get; set; }
        public Dictionary<string, object> Parametros { get; set; }

        public void SaveEvents()
        {
            try
            {
                //Text File Path
                string filepath = System.Web.HttpContext.Current.Server.MapPath("~/AuditFiles/");
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                //Text File Name
                filepath += $"EventLog {DateTime.Now.ToString("dd-MM-yyyy")}.txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("DateTime : " + DateTime.Now.ToString());
                    sw.WriteLine();
                    sw.WriteLine("UserId   : " + Id);
                    sw.WriteLine("UserName : " + Usuario);
                    sw.WriteLine("Page Url : " + Area_acceso);
                    sw.WriteLine("Host IP  : " + Direccion_ip);
                    sw.WriteLine("Browser  : " + Navegador);
                    sw.WriteLine("Details  : " + Detalle);
                    sw.WriteLine("UserAgent: " + UAgente);
                    foreach (var parameter in Parametros)
                    {
                        sw.WriteLine("Key      : " + parameter.Key);
                        sw.WriteLine("Value    : " + parameter.Value);
                    }
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        public string Messsage { get; set; }
        public string InnerExceptionMessage { get; set; }
        public string NumException { get; set; }
        public string ExceptionName { get; set; }
        public string ExceptionType { get; set; }

        public void SaveException()
        {
            try
            {
                string filepath = System.Web.HttpContext.Current.Server.MapPath("~/AuditFiles/");
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath += $"Exception {DateTime.Now.ToString("dd-MM-yyyy")}.txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("DateTime: " + DateTime.Now.ToString());
                    sw.WriteLine();
                    sw.WriteLine("UserId  : " + Id);
                    sw.WriteLine("UserName: " + Usuario);
                    sw.WriteLine("Line No.: " + NumException);
                    sw.WriteLine("Exception Message      : " + Messsage);
                    sw.WriteLine("InnerException Message : " + InnerExceptionMessage);
                    sw.WriteLine("Exception Name  : " + ExceptionName);
                    sw.WriteLine("Exception Type  : " + ExceptionType);
                    sw.WriteLine("Page Url: " + Area_acceso);
                    sw.WriteLine("Host IP : " + Direccion_ip);
                    sw.WriteLine("Browser : " + Navegador);
                    sw.WriteLine("Details : " + Detalle);
                    foreach (var parameter in Parametros)
                    {
                        sw.WriteLine("Key      : " + parameter.Key);
                        sw.WriteLine("Value    : " + parameter.Value);
                    }
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

    }

    // Auditoría  MVC
    public class AuditFilter : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Stores the Request in an Accessible object           
            var request = filterContext.HttpContext.Request;
            var _user = filterContext.HttpContext.User.Identity;

            // Dictionary<key, value> Parameters
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var parameter in filterContext.ActionParameters)
            {
                parameters.Add(parameter.Key, Newtonsoft.Json.JsonConvert.SerializeObject(parameter.Value));
            }

            // Generate an audit
            Audit audit = new Audit();
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
            audit.SaveEvents();

            // Finishes executing the Action as normal 
            base.OnActionExecuting(filterContext);
        }
    }

    // Auditoría API
    public class AuditFilterApi : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Stores the Request in an Accessible object           
            var request = HttpContext.Current.Request;
            var _user = actionContext.RequestContext.Principal.Identity;

            // Dictionary<key, value> Parameters
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var parameter in actionContext.ActionArguments)
            {
                parameters.Add(parameter.Key, Newtonsoft.Json.JsonConvert.SerializeObject(parameter.Value));
            }

            // Generate an audit
            Audit audit = new Audit();
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
            audit.SaveEvents();

            // Finishes executing the Action as normal 
            base.OnActionExecuting(actionContext);
        }
    }
}