using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
    [RoutePrefix("api/Direccion")]
    public class DireccionController : ApiController
    {
        [HttpGet]
        [Route("Estados")]
        public IHttpActionResult Estados()
        {
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                List<Entities.Registro.Estado> estados = (from s in context.sepomex
                                                          select new Entities.Registro.Estado
                                                          {
                                                              Id = s.estado,
                                                              Descripcion = s.estado

                                                          }).Distinct().OrderBy(e => e.Id).ToList();               
                return Json(estados);
            }
        }

        [HttpGet]
        [Route("Municipios")]
        public IHttpActionResult Municipios(string estado)
        {
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                List<Entities.Registro.Municipio> municipios = (from s in context.sepomex
                                                                where (s.estado == estado || estado == null)
                                                                select new Entities.Registro.Municipio
                                                                {
                                                                    Id = s.municipio,
                                                                    Descripcion = s.municipio
                                                                }).Distinct().OrderBy(m => m.Id).ToList();
                return Json(municipios);
            }
        }

        [HttpGet]
        [Route("Colonias")]
        public IHttpActionResult Colonias(string estado, string municipio, string codigopostal)
        {
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                List<Entities.Registro.Colonia> colonias = (from s in context.sepomex
                                                            where (s.estado == estado || estado == null)
                                                            && (s.municipio == municipio || municipio == null)
                                                            && (s.codigo_postal == codigopostal || string.IsNullOrEmpty(codigopostal))
                                                            select new Entities.Registro.Colonia
                                                            {
                                                                Id = s.colonia,
                                                                Descripcion = s.colonia
                                                            }).Distinct().OrderBy(c => c.Id).ToList();
                return Json(colonias);
            }
        }

        [HttpGet]
        [Route("CodigoPostal")]
        public IHttpActionResult CodigoPostal(string estado, string municipio, string colonia)
        {
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                var resultCodigo = (from s in context.sepomex
                                    where s.estado == estado
                                    && s.municipio == municipio
                                    && s.colonia == colonia
                                    select new
                                    {
                                        CodigoPostal = s.codigo_postal,
                                        Ciudad = s.ciudad
                                    }).FirstOrDefault();
                return Json(resultCodigo);
            }
        }

        //public JsonResult TipoParticipante()
        //{
        //    using (Modelo.Plaza_Entities context = new Modelo.Plaza_Entities())
        //    {
        //        List<Entidades.Participante.TipoParticipante> tipo = (from p in context.tipo_participante
        //                                                              select new Entidades.Participante.TipoParticipante
        //                                                              {
        //                                                                  Id = p.id,
        //                                                                  Descripcion = p.descripcion
        //                                                              }).Distinct().OrderBy(e => e.Id).ToList();
        //        return Json(tipo, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        [Route("BusquedaCP")]
        public IHttpActionResult BusquedaCP(string codigo_postal)
        {
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                var result = context.sepomex.Where(s => s.codigo_postal == codigo_postal).Select(s => new
                {
                    s.estado,
                    s.municipio,
                    s.colonia,
                    Colonias = new
                    {
                        Id = s.colonia,
                        Descripcion = s.colonia
                    }
                }).ToList();
                return Json(result);
            }
          
        }

        [HttpGet]
        [Route("Sexo")]
        public IHttpActionResult Sexo()
        {

            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                var result = context.sexo.Select(s => new
                {
                    s.id,
                    s.descripcion
                }).OrderByDescending(s => s.id).ToList();

                return Json(result);
            }
        }
        [HttpGet]
        [Route("Lista/Ocupacion")]
        public IHttpActionResult lOcupacion()
        {
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                var result = context.ocupacion.Select(s => new
                {
                    s.id,
                    s.descripcion,
                    s.descripcion_larga
                }).OrderByDescending(s => s.id).ToList();

                return Json(result);
            }
        }

    }
}
