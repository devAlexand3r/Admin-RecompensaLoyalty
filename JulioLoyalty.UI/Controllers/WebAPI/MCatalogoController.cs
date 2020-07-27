using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
    [RoutePrefix("api/MCatalogo")]
    public class MCatalogoController : ApiController
    {
        [HttpGet]
        [Route("Lealtad")]
        public IHttpActionResult lealtad()
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var result = db.catalogos_lealtad.Select(s => new
                {
                    s.id,
                    s.clave,
                    s.descripcion,
                    s.descripcion_larga,
                    s.fecha_alta
                }).OrderBy(s => s.descripcion).ToList();
                return Json(result);
            }
        }

        [HttpGet]
        [Route("RCatalogos")]
        public string rCatalogos(string catalogo)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@Catalogo", catalogo);
                DataSet setTables = db.GetDataSet("[dbo].[usp_buscar_catalogos]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [HttpGet]
        [Route("CEsquema")]
        public string cEsquema(string catalogo)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@Catalogo", catalogo);
                DataSet setTables = db.GetDataSet("[dbo].[usp_esquema_catalogo]", CommandType.StoredProcedure, parameters);                
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }
        
        [HttpGet]
        [Route("FKCatalogos")]
        public string fkCatalogos(string descripcion)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@catalogo", descripcion);
                DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_catalogo]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [HttpPost]
        [Route("AgregarMCatalogo")]
        public IHttpActionResult agregarMCatalogo(SQLParameters parameters)
        {
            parameters.userName = User.Identity.Name;
            Business.MCatalogos.IMantenimiento mantenimiento = new Business.MCatalogos.Mantenimiento();
            return Json(mantenimiento.AgregarMantenimiento(parameters));
        }

        [HttpPost]
        [Route("ActualizarMCatalogo")]
        public IHttpActionResult actualizarMCatalogo(SQLParameters parameters)
        {
            parameters.userName = User.Identity.Name;
            Business.MCatalogos.IMantenimiento mantenimiento = new Business.MCatalogos.Mantenimiento();
            return Json(mantenimiento.ActualizarMantenimiento(parameters));
        }

        [HttpPost]
        [Route("EliminarMCatalogo")]// Utilizar únicamente cuando se dese eliminar definitivamente del objeto
        public IHttpActionResult eliminarMCatalogo(SQLParameters parameters)
        {
            parameters.userName = User.Identity.Name;
            Business.MCatalogos.IMantenimiento mantenimiento = new Business.MCatalogos.Mantenimiento();
            return Json(mantenimiento.EliminarCEMantenimiento(parameters));
        }

    }
}
