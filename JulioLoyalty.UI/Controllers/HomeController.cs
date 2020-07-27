using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using JulioLoyalty.Model;
using System.Threading.Tasks;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Gerente"))
            {
                return RedirectToAction("Activaciones", "Report");
            }
            return View();
        }

        public async Task<ActionResult> About()
        {
            ViewBag.Message = "Your application description page.";

            //Business.EmailService.MailChimp.csMailChimp _emailChimp = new Business.EmailService.MailChimp.csMailChimp();
            //Business.EmailService.MailChimp.CampaignSettings settings = new Business.EmailService.MailChimp.CampaignSettings();
            //settings = _emailChimp.getSettings("CP");
            //settings.SubjectLine = "Prueba de alerta";
            //string listid = _emailChimp.getListId("LAA");
            //await _emailChimp.sendAlert(settings, listid, "~/Templates/new_user.html", DateTime.Now);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}