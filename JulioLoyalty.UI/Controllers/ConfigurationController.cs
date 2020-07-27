using JulioLoyalty.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class ConfigurationController : Controller
    {
        // GET: Configuration
        public ActionResult Index()
        {
            return View();
        }

        // GET: Roles
        public ActionResult Roles()
        {
            return View();
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // GET: Function
        public ActionResult Function()
        {
            return View();
        }

        // GET: Menu
        public ActionResult Menu()
        {
            return View();
        }


    }
}