using JulioLoyalty.Business;
using JulioLoyalty.UI.Models.CallCenter;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            int campaign_id = 0;
            if (Session[User.Identity.GetUserId()] != null)
                campaign_id = (int)Session[User.Identity.GetUserId()];

            var model = loadModel(0, campaign_id);
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Index(int id)
        {
            var camp_id = Request.Form["campaña_id"];
            int campaign_id = string.IsNullOrEmpty(camp_id) ? 0 : Convert.ToInt32(camp_id);
            Session[User.Identity.GetUserId()] = campaign_id;

            unit.UpdateScheduleStatus(id);
            var model = loadModel(id, campaign_id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult SaveChanges(Entities.Modulos.Llamada.Models.FormLog model)
        {
            model.Status = unit.GetCallStatusList();
            model.Telefonos = unit.GetPhoneNumberList(model.LogId);

            if (ModelState.IsValid)
            {
                try
                {
                    unit.SaveLogs(model);
                    ModelState.AddModelError("OK", "¡Genial! Registro exitoso");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }
            }

            return PartialView("Forms/_FormLog", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult SaveSchedule(Entities.Modulos.Llamada.Models.FormSchedule model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    unit.SaveSchedule(model);
                    ModelState.AddModelError("OK", "¡Genial! Registro exitoso");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }
            }
            return PartialView("Forms/_FormSchedule", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult SearchParticipant(Entities.Modulos.Llamada.Models.FormSearch model)
        {
            model.Campaigns = unit.GetCampaignList();
            if (ModelState.IsValid)
            {
                try
                {
                    model.SearchResult = unit.GetParticipantList(model.Parameters, model.Campaign_id == null ? 0 : Convert.ToDecimal(model.Campaign_id));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }
            }

            return PartialView("Forms/_FormSearch", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult SearchParticipantLog(Entities.Modulos.Llamada.Models.FormSearchLog model)
        {
            model.Campaigns = unit.GetCampaignList();
            if (ModelState.IsValid)
            {
                try
                {
                    model.SearchResult = unit.GetParticipantLogList(model.Parameters, model.Campaign_id == null ? 0 : Convert.ToDecimal(model.Campaign_id));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }
            }

            return PartialView("Forms/_FormSearchLog", model);
        }

        #region Aplicaciones auxiliares
        public ICallUnit unit
        {
            get { return new CallUnit(User.Identity.GetUserId()); }
        }

        /// <summary>
        /// Cargar modelo (Participante_campaña_id, campaña_id)
        /// </summary>
        public Entities.Modulos.Llamada.cModeloPrincipal loadModel(int id, int camp_id)
        {
            var participant_campaign_id = id > 0 ? id : unit.GetParticipantCampaignId(camp_id);
            var participant = unit.GetParticipant(participant_campaign_id);
            Entities.Modulos.Llamada.cModeloPrincipal model = new Entities.Modulos.Llamada.cModeloPrincipal();
            model.campaña_id = camp_id;
            if (participant != null)
            {
                model.Id = participant.Id; // participante_campaña (id)
                model.Nombre = participant.Nombre;
                model.campaña_id = participant.campaña_id;
            }
            model.Telefonos = unit.GetPhoneNumberList(participant_campaign_id);
            model.Status = unit.GetCallStatusList();
            model.Citas = unit.GetMeetingDateList();
            model.Campañas = unit.GetCampaignList();
            model.Count_Campania_Llamadas = unit.GetCountCampaniaLlamadas(model.campaña_id);
            if (model.campaña_id != 0)
            {
                foreach (var item in model.Campañas)
                {
                    if (item.id == model.campaña_id)
                    {
                        model.Script = item.script;
                        break;
                    }
                }
            }
            return model;
        }
        #endregion
    }
}