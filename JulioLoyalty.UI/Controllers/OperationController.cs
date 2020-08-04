using JulioLoyalty.Business.Configuracion;
using JulioLoyalty.Business.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class OperationController : Controller
    {
        List<SelectListItem> selectListItems = new List<SelectListItem>();
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

        //GET: Pais
        public async Task<ActionResult> Pais()
        {
            RequestPais model = await GetPais();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Pais(RequestPais pais)
        {
            await GetPais();
            if (!ModelState.IsValid)
            {
                return View(pais);
            }
            IConfiguracion configuracion = new Configuracion();
            var result = await configuracion.UpdatePaisAsync(pais);
            ViewBag.result = "ok";
            return View(result);
        }

        private async Task<RequestPais> GetPais()
        {
            IConfiguracion configuracion = new Configuracion();
            var temas = await configuracion.GetTemaListAsync();
            var model = await configuracion.GetPaisAsync();
            List<SelectListItem> temaslist = temas.Select(m => new SelectListItem() { Value = m.clave, Text = m.descripcion }).ToList();
            Select(temaslist, model.theme);
            ViewBag.temas = selectListItems;
            Session.Remove("pais");
            return model;
        }
        private void Select(List<SelectListItem> selectLists, string theme)
        {
            foreach (var item in selectLists)
            {
                if (item.Value == theme)
                {
                    item.Selected = true;
                }
                selectListItems.Add(item);
            }
        }
    
        public RequestPais GetPaisBy()
        {
            IConfiguracion configuracion = new Configuracion();
            var model = Task.Run(async () => await configuracion.GetPaisAsync()).GetAwaiter().GetResult();
            return model;
        }
    }
}