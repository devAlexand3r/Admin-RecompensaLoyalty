using JulioLoyalty.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Mvc;
using JulioLoyalty.Business.Funciones;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Business.Campania;
using JulioLoyalty.Entities.MailChimp;
using JulioLoyalty.Entities.Campaign;
using JulioLoyalty.Business;
using Microsoft.AspNet.Identity;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {
        // GET: Campain
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Filter(int id, string temporada)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", id);
                parameters.Add("@temporada", temporada);
                DataTable dtCatalogo = db.GetDataSet("[dbo].[usp_queryBuilder_filters]", CommandType.StoredProcedure, parameters).Tables[0];
                return Json(JsonConvert.SerializeObject(dtCatalogo), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Data(string par)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@sql", par);
                DataTable dtCatalogo = db.GetDataSet("[dbo].[usp_queryBuilder_result]", CommandType.StoredProcedure, parameters).Tables[0];

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dtCatalogo.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dtCatalogo.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                return Json(rows, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult DataCnt(string par)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@sql", par);
                DataTable dtCatalogo = db.GetDataSet("[dbo].[usp_queryBuilder_registros]", CommandType.StoredProcedure, parameters).Tables[0];                
                return Json(dtCatalogo.Rows[0]["registros"], "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveSegment(string prDescription, string prLongDescription, string prSQL)
        {
            var prResult = new Campania().SaveSegment(prDescription, prLongDescription, prSQL, User.Identity.GetUserId());
            return Json(prResult);
        }

        [HttpGet]
        public string ConsultaCampaniasPendientes()
        {
            // Obtener las Campañas que no tienen asociada una lista y con Status Save
            csMailChimp campaign = new csMailChimp();
            cListCampaign.RootObject parsed = new cListCampaign.RootObject();
            cCampaignWithoutList campaign_without_list = new cCampaignWithoutList();
            campaign_without_list.lstCampaingWithoutList = new List<cCampaignWithoutList.CampaignWithoutList>();
            Int32 count = campaign.GetCampaignCount(ConfigurationManager.AppSettings["DataCenter"].ToString(), ConfigurationManager.AppSettings["MailChimpApiKey"].ToString());
            Int32 offset = 0;
            Int32 intervalo;
            if (count > Int32.Parse(ConfigurationManager.AppSettings["cntCampaign"]))
            {
                intervalo = count / Int32.Parse(ConfigurationManager.AppSettings["cntCampaign"]);
                count = Int32.Parse(ConfigurationManager.AppSettings["cntCampaign"]);
            }
            else
            {
                intervalo = 0;                
            }
            for (Int32 i = 0; i <= intervalo; i++)
            {
                string resp = campaign.GetCampaignStatusList(ConfigurationManager.AppSettings["DataCenter"].ToString(), ConfigurationManager.AppSettings["MailChimpApiKey"].ToString(), "Save", count, offset);
                parsed = (cListCampaign.RootObject)JsonConvert.DeserializeObject(resp, typeof(cListCampaign.RootObject));
                string title = string.Empty;
                foreach (var item in parsed.campaigns)
                {
                    if (item.recipients.list_id == string.Empty) // Significa que la campaña no tiene asociada una lista y puede aparecer vacio el list_id
                    {
                        if (item.settings.title == string.Empty) // A veces viene vacio el titulo de la campaña pendiente
                            title = "Sin Titulo - Campaign_id " + item.id;
                        else
                            title = item.settings.title;
                        campaign_without_list.lstCampaingWithoutList.Add(new cCampaignWithoutList.CampaignWithoutList
                        {
                            campaign_id = item.id,
                            name = title
                        });
                    }
                }
                offset = offset + Int32.Parse(ConfigurationManager.AppSettings["cntCampaign"]);
            }
            // Por tener la tabla campaign actualizada se basa en el de la Campaña de MailChimp
            campaign.UpdateTableCampaign(campaign_without_list.lstCampaingWithoutList);
            //
            return JsonConvert.SerializeObject(campaign_without_list.lstCampaingWithoutList);
        }

        [HttpGet]
        public string ConsultaCampaniasLlamadas()
        {
            cCampaignList campania = new cCampaignList();
            Campania campaign = new Campania();
            campania = campaign.consultaCampanias();
            return JsonConvert.SerializeObject(campania.lstCampaingList);
        }

        [HttpGet]
        public string ConsultaUsuariosLlamadas()
        {
            cUsuariosLlamadas usuarios_llamadas = new cUsuariosLlamadas();
            Campania llamadas = new Campania();
            usuarios_llamadas = llamadas.consultaUsuariosLlamadas();
            return JsonConvert.SerializeObject(usuarios_llamadas.lstUsuariosLlamadas);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue // Use this value to set your maximum size for all of your Requests
            };
        }

        [HttpPost]
        public JsonResult Aceptar(RequestGeneraCampania request)
        {
            IRepositoryCampania campania = new Campania();
            request.UserName = User.Identity.Name;
            JsonResult data = Data(request.query);
            // Convertir Json a Clase Miembros
            var json = JsonConvert.SerializeObject(data.Data);
            request.miembros = JsonConvert.DeserializeObject<List<RequestGeneraCampania.Lista>>(json);
            return Json(campania.GeneraCampania(request, ConfigurationManager.AppSettings["DataCenter"].ToString(), ConfigurationManager.AppSettings["MailChimpApiKey"].ToString(), User.Identity.GetUserId()));
        }

        [HttpPost]
        public JsonResult AceptarLlamada(RequestGeneraLlamada request)
        {
            IRepositoryCampania campania = new Campania();
            JsonResult data = Data(request.query);
            // Convertir Json a Clase Miembros
            var json = JsonConvert.SerializeObject(data.Data);
            request.miembros = JsonConvert.DeserializeObject<List<RequestGeneraLlamada.Lista>>(json);
            return Json(campania.GeneraLlamada(request, User.Identity.GetUserId()));
        }
    }
}