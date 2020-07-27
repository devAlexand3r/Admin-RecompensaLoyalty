using JulioLoyalty.Business.EmailService;
using JulioLoyalty.Entities;
using JulioLoyalty.Model;
using JulioLoyalty.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JulioLoyalty.UI.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AccountController() { }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { _roleManager = value; }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToRoleInitPage(model.UserName);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                    return View(model);
            }
        }

        private ActionResult RedirectToRoleInitPage(string UserName)
        {
            var user = UserManager.FindByName(UserName);
            var userRole = UserManager.GetRoles(user.Id)[0];

            string roleInitialPageUrl = RoleManager.Roles.Where(r => r.Name == userRole).FirstOrDefault().InitialPageUrl;
            return Redirect(roleInitialPageUrl);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Setting()
        {
            return View();
        }



        [HttpPost]
        public JsonResult ChangePassword(JulioLoyalty.Business.Parameters.RequesUser user)
        {
            JulioLoyalty.Business.ResultJson rJon = new Business.ResultJson();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ApplicationUser cUser = UserManager.FindById(user.Key);
                    cUser.PasswordHash = UserManager.PasswordHasher.HashPassword(user.Password);
                    var result = UserManager.Update(cUser);

                    rJon.Success = result.Succeeded;
                    rJon.Message = "La contraseña se cambio con exito";
                    if (!result.Succeeded)
                        rJon.Message = "No podimos actualizar su contraseña, intente mas tarde";

                    return Json(rJon);
                }
            }
            catch (Exception ex)
            {
                rJon.Success = false;
                rJon.Message = ex.Message;
                return Json(rJon);
            }
        }


        public ActionResult Recover()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Recover(RecoverModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = UserManager.FindByNameAsync(model.UserName).Result;
            if (user == null || !(UserManager.IsEmailConfirmedAsync(user.Id).Result))
            {
                ViewBag.Message = "¡Error al restablecer su contraseña!";
                return View(model);
            }

            var token = UserManager.GeneratePasswordResetTokenAsync(user.Id).Result;
            var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, protocol: HttpContext.Request.Url.Scheme);
            // Enviar Url para cambiar la contraseña
            EParameters parameter = new EParameters()
            {
                IdUnique = 1,
                FullName = $"{user.Profiles.FirstName} {user.Profiles.MiddleName} {user.Profiles.LastName}",
                Email = user.Email,
                Link = resetLink,
                currentTime = DateTime.Now
            };
            IRepositoryEmail service = new Email(new Templates(1), parameter);
            service.Submit();

            ViewBag.Message = "¡Se ha enviado un enlace para restablecer su contraseña a su dirección de correo electrónico¡";
            return View(model);
        }

        // GET: /Account/ResetPassword
        public ActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePasswordAjax(JulioLoyalty.UI.Models.ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                        var token = UserManager.GeneratePasswordResetTokenAsync(user.Id).Result;
                        IdentityResult result = UserManager.ResetPasswordAsync(user.Id, token, model.Password).Result;
                        if (result.Succeeded)
                        {
                            user.EmailConfirmed = false;
                            //if (string.IsNullOrEmpty(user.Email))
                            //{
                            //    user.Email = "contacto@julioloyalty.com";
                            //}
                            await UserManager.UpdateAsync(user);
                            model.Password = string.Empty;
                            model.ConfirmPassword = string.Empty;
                            ModelState.AddModelError("OK", "La contraseña se cambio exitosamente");
                        }
                        else
                        {
                            foreach (string error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No encontramos ninguna cuenta asociada con el No. de membresía");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return PartialView("_ChangePassword", model);
        }


        [HttpGet]
        public JsonResult ResetOnlyPassword()
        {
            var participantes = Business.UnitOfWork.GetAspNetUsers(User.Identity.GetUserId());
            List<string> errors = new List<string>();
            foreach (var item in participantes)
            {
                var user = UserManager.FindByName(item.clave);
                if (user != null)
                {
                    try
                    {
                        UserManager.RemovePassword(user.Id);
                        string token = UserManager.GeneratePasswordResetToken(user.Id);
                        string password = item.fecha_nacimiento.ToString("dd/MM/yyyy");
                        IdentityResult result = UserManager.ResetPassword(user.Id, token, password);
                        if (result.Succeeded)
                        {
                            user.EmailConfirmed = false;                           
                            UserManager.Update(user);
                        }
                        else
                        {
                            foreach (string error in result.Errors)
                            {
                                errors.Add(error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return Json(new { errors = errors.ToList(), data = string.Format("Total de registros {0} procesados", participantes.Count().ToString()) }, JsonRequestBehavior.AllowGet);
        }




        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Message = "¡La contraseña de confirmación es incorrecta¡";
                return View();
            }

            ApplicationUser user = UserManager.FindByNameAsync(model.UserName).Result;
            if (user == null)
            {
                ViewBag.Message = "El nombre de usuario no fue encontrado";
                return View();
            }
            IdentityResult result = UserManager.ResetPasswordAsync(user.Id, model.Token, model.Password).Result;
            if (result.Succeeded)
            {
                ViewBag.Message = "¡Restablecimiento de contraseña exitoso!";
                return View();
            }
            else
            {
                ViewBag.Message = "¡Error al restablecer la contraseña!";
                return View();
            }
        }

        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}