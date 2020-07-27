using JulioLoyalty.Business.Mecanica;
using JulioLoyalty.Entities.Mecanica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{

    [Authorize]
    public class MecanicaController : ApiController
    {
        private readonly MecanicaImplementacion mecanica = new MecanicaImplementacion();

        [HttpGet]
        public IHttpActionResult Nivel()
        {
            var data = mecanica.MecanicaNivel();
            return Json(data);
        }

        [HttpPost]
        public IHttpActionResult AgregarNivel(MecanicaNivel mecanicaNivel)
        {
            Dictionary<string, object> error = new Dictionary<string, object>();
            try
            {
                if (ModelState.IsValid)
                {
                    var respuesta = mecanica.AgregarMecanicaNivel(mecanicaNivel);
                    error.Add("Code", 200);
                    error.Add("Message", respuesta);
                    return Json(error);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                error.Add("Code", 500);
                error.Add("Message", ex.Message);
                return Json(error);
            }
        }

        [HttpPut]
        public IHttpActionResult ActualizarNivel(MecanicaNivel mecanicaNivel)
        {
            Dictionary<string, object> error = new Dictionary<string, object>();
            try
            {
                if (ModelState.IsValid)
                {
                    var respuesta = mecanica.ActualizarMecanicaNivel(mecanicaNivel);
                    error.Add("Code", 200);
                    error.Add("Message", respuesta);
                    return Json(error);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                error.Add("Code", 500);
                error.Add("Message", ex.Message);
                return Json(error);
            }
        }

    }
}

