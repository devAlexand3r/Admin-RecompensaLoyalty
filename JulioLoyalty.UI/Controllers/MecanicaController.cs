using JulioLoyalty.Business.Mecanica;
using JulioLoyalty.Entities.Mecanica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class MecanicaController : Controller
    {
        // GET: Mecanica
        public ActionResult Index()
        {
            return View();
        }
    }
}