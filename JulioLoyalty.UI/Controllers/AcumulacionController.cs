using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class AcumulacionController : Controller
    {
        // GET: Acumulacion
        public ActionResult Index()
        {
            return View();
        }
    }
}