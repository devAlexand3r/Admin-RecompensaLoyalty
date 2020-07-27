using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
	[Authorize]
	public class OperationController : Controller
	{
		// GET: Operation
		public ActionResult Index()
		{
			//// Pruebas de Plantillas de Bienvenida
			//Business.Funciones.envioMail envio = new Business.Funciones.envioMail();
			//envio.envioMailParticipante("~/Plantillas/avisoBienvenida100.html", "¡Bienvenida a JULIO Loyalty! ¡Has recibido 100 puntos!", "Gloria", "gloria.nieves@lms-la.com");
			//envio.envioMailParticipante("~/Plantillas/avisoBienvenida10Porc.html", "¡Bienvenida a JULIO Loyalty! ¡Recibiste un bono especial!", "Gloria", "gloria.nieves@lms-la.com");
			//envio.envioMailParticipante("~/Plantillas/avisoBienvenida5Porc.html", "¡Bienvenida a JULIO Loyalty!", "Gloria", "gloria.nieves@lms-la.com");

			return View();
		}

		// GET: General
		public ActionResult General()
		{
			return View();
		}

		// GET: transaction
		public ActionResult Transaction()
		{
			return View();
		}

		// GET: Catalog
		public ActionResult Catalog()
		{
			return View();
		}

		// GET: AddTransaction
		public ActionResult AddTransaction()
		{
			return View();
		}
		// GET: Exchange
		public ActionResult Exchange()
		{
			return View();
		}

		// GET: Update
		public ActionResult Update()
		{
			return View();
		}

		// GET: Activation
		public ActionResult Activation()
		{
			return View();
		}

		// GET: Accumulation
		public ActionResult Accumulation()
		{
			return View();
		}
		
		// GET: Accumulation
		public ActionResult Ticket()
		{
			return View();
		}

        // GET: Cancellation
        public ActionResult Cancellation()
        {
            return View();
        }


    }
}