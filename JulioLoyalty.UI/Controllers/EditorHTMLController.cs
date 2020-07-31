using JulioLoyalty.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    [Authorize]
    public class EditorHTMLController : Controller
    {
        // GET: EditorHTML
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewHtml()
        {
            return View();
        }

        public async Task<ActionResult> getHtml(htmlPais html)
        {
            IEditorHTML editorHTML = new EditorHTML();
            var _html=await editorHTML.GetHTML(html);
            Session["Result"] = null;
            return Json(_html, JsonRequestBehavior.AllowGet);
        }

        [HttpPost,ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveHtml(htmlPais htmlSelect, string htmlText)
        {
            IEditorHTML editorHTML = new EditorHTML();
            var _html = await editorHTML.SaveHtml(htmlSelect, htmlText);
            Session["Result"] = _html;
            return RedirectToAction("Index");
        }
    }
}