using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    public class LlamadaController : Controller
    {
        // GET: Llamada
        public ActionResult Captura()
        {
            return View();
        }
		public ActionResult Seguimiento()
		{
			return View();
		}
		public ActionResult Historico()
		{
			return View();
		}
		public ActionResult Envio()
		{
			return View();
		}	
	}
}