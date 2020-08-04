using JulioLoyalty.Business.Customer;
using JulioLoyalty.Entities.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    public class TemaClienteController : Controller
    {
        // GET: TemaCliente
            List<SelectListItem> selectListItems = new List<SelectListItem>();
        public TemaClienteController()
        {
            selectListItems.Add(new SelectListItem() { Value = "theme-cafe", Text = "Cafe" });
            selectListItems.Add(new SelectListItem() { Value = "theme-gris", Text = "Gris" });
            selectListItems.Add(new SelectListItem() { Value = "theme-dorado", Text = "Dorado" });
            selectListItems.Add(new SelectListItem() { Value = "theme-rojo", Text = "Rojo" });
            selectListItems.Add(new SelectListItem() { Value = "theme-negro", Text = "Negro" });
            selectListItems.Add(new SelectListItem() { Value = "theme-rosita", Text = "Rosita" });

        }
        public ActionResult Index()
        {
            ViewBag.option = selectListItems;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThemeUpdate(string theme)
        {
            IRepositoryCustomer repositoryCustomer = new Customer();
            var _theme = await repositoryCustomer.ThemeAsync(theme);
            return Json(_theme);
        }
    }
}