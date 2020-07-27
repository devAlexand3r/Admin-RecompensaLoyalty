using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public ActionResult Activaciones()
        {
            return View();
        }

        public ActionResult Actividad()
        {
            return View();
        }

        public ActionResult Llamadas()
        {
            return View();
        }

        public ActionResult Fraude()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        public ActionResult Compras()
        {
            return View();
        }

        public ActionResult Visitas()
        {
            return View();
        }

        public ActionResult Tickets()
        {
            return View();
        }

        public ActionResult Ventas()
        {
            return View();
        }

        public ActionResult Puntos()
        {
            return View();
        }

        public ActionResult Week()
        {
            return View();
        }

        [HttpGet]
        public JsonResult EveryWeek()
        {
            var data = JulioLoyalty.Business.UnitOfWork.EveryWeek(User.Identity.GetUserId());
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Buy()
        {
            return View();
        }

        [HttpGet]
        public JsonResult buyData(string dateStart, string dateEnd)
        {
            var result = Business.UnitOfWork.BuyDataResult(Convert.ToDateTime(dateStart), Convert.ToDateTime(dateEnd));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Activation()
        {
            return View();
        }

        [HttpGet]
        public JsonResult activationData()
        {
            var result = Business.UnitOfWork.partDataResult();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}