using JulioLoyalty.Business.Customer;
using JulioLoyalty.Business.Llamada;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
	[RoutePrefix("api/Llamada")]
	public class LlamadaController : ApiController
	{
		[HttpPost]
		[Route("Envio")]
		public IHttpActionResult Envio(RequestEnvio request)
		{
			IRepositoryLlamada llamada = new Llamada();
			request.UserName = User.Identity.Name;
			return Json(llamada.Envio(request));
		}

		[HttpGet]
		[Route("Historico")]
		public string Historico(decimal? participante_id)
		{
			Llamada llamada = new Llamada();
			return llamada.Historico(participante_id);
		}

		[HttpGet]
		[Route("ConsultaLlamada")]
		public string ConsultaLlamada(decimal? llamada_id)
		{
			Llamada llamada = new Llamada();
			return llamada.ConsultaLlamada(llamada_id);
		}

		[HttpGet]
		[Route("HistoricoDetalle")]
		public string HistoricoDetalle(decimal? llamada_id)
		{
			Llamada llamada = new Llamada();
			return llamada.HistoricoDetalle(llamada_id);
		}

		[HttpGet]
		[Route("CargaStatusSeguimientoAbierto")]
		public string CargaStatusSeguimientoAbierto()
		{
			Llamada llamada = new Llamada();
			return llamada.CargaStatusSeguimientoAbierto();
		}

		[HttpGet]
		[Route("CargaTelefono")]
		public string CargaTelefono(decimal? participante_id)
		{
			Llamada llamada = new Llamada();
			return llamada.CargaTelefono(participante_id);
		}

		[HttpGet]
		[Route("CargaTipoLlamada")]
		public string CargaTipoLlamada()
		{
			Llamada llamada = new Llamada();
			return llamada.CargaTipoLlamada();
		}

		[HttpPost]
		[Route("Captura")]
		public IHttpActionResult Captura(RequestCaptura request)
		{
			IRepositoryLlamada llamada = new Llamada();
			request.UserName = User.Identity.Name;			
			return Json(llamada.Captura(request));
		}

		[HttpGet]
		[Route("Seguimiento")]
		public string Seguimiento(decimal? participante_id)
		{
			Llamada llamada = new Llamada();
			return llamada.Seguimiento(participante_id);
		}

		[HttpGet]
		[Route("SeguimientoDetalle")]
		public string SeguimientoDetalle(decimal? id)
		{
			Llamada llamada = new Llamada();
			return llamada.SeguimientoDetalle(id);
		}

		[HttpGet]
		[Route("Escalamiento")]
		public IHttpActionResult Escalamiento()
		{
			using (Model.DbContextJulio context = new Model.DbContextJulio())
			{
				var query = (from tll in context.tipo_llamada
							 join ctll in context.categoria_tipo_llamada on tll.categoria_tipo_llamada_id equals ctll.id
							 where (ctll.clave == "ESC")
							 select new
							 {
								 tll.id,
								 Descripcion = tll.descripcion
							 }).ToList();
				List<Entities.Escalamiento> escalamiento = new List<Entities.Escalamiento>();
				foreach (var item in query)
				{
					escalamiento.Add(new Entities.Escalamiento { Id = item.id.ToString(), Descripcion = item.Descripcion });
				};
				return Json(escalamiento);
			}
		}

		[HttpGet]
		[Route("Status")]
		public IHttpActionResult Status()
		{
			using (Model.DbContextJulio context = new Model.DbContextJulio())
			{
				List<Entities.Status> status = (from s in context.status_seguimiento
												where (s.clave == "PJ" || s.clave == "PL" || s.clave == "CR" || s.clave == "CN") // Proceso Julio, Proceso LMS, Atendido, Cerrado Satisfactorio ó Cerrado no Satisfactorio
												select new Entities.Status
												{
													Id = s.id.ToString(),
													Descripcion = s.descripcion
												}).Distinct().OrderBy(s => s.Id).ToList();
				return Json(status);
			}
		}

		[HttpPost]
		[Route("GuardarSeguimiento")]
		public IHttpActionResult GuardarSeguimiento(RequestSeguimiento request)
		{
			IRepositoryLlamada llamada = new Llamada();
			request.UserName = User.Identity.Name;
			return Json(llamada.Seguimiento(request));
		}
	}
}