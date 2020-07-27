using JulioLoyalty.Model;
using JulioLoyalty.UI.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
    [Authorize]
    [RoutePrefix("api/ticket")]
    public class TicketController : ApiController
    {
        [HttpGet]
        [Route("test")]
        [AuditFilterApi, APIExceptionFilter]
        public string Get()
        {
            string dat = "32-12-2018";
            DateTime setDate = Convert.ToDateTime(dat);
            return "Wep Api";
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpPost]
        [Route("Cancel")]
        public string Post([FromBody]cancelar_tickect request)
        {
            if (string.IsNullOrEmpty(request.tarjeta) || request.distribuidor_id == 0 || request.status_participante_id == 0)
                return null;

            using (DbContextJulio db = new DbContextJulio())
            {
                var userId = User.Identity.GetUserId();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@tarjeta", request.tarjeta);
                parameters.Add("@distribuidor_id", request.distribuidor_id);
                parameters.Add("@status_participante_id", request.status_participante_id);
                parameters.Add("@usuario_id", userId);
                DataSet setTables = db.GetDataSet("[dbo].[usp_cancelar_tarjeta]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }
    }

    public class cancelar_tickect
    {
        public string tarjeta { get; set; }
        public decimal distribuidor_id { get; set; }
        public decimal status_participante_id { get; set; }
    }
}
