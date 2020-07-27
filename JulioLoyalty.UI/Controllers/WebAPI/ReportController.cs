using JulioLoyalty.Model;
using JulioLoyalty.UI.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
	[RoutePrefix("api/Report")]
	public class ReportController : ApiController
	{
		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("Participantes")]
		public string participantes(string fecha_inicial, string fecha_final, decimal distribuidor_id, decimal status_participante_id)
		{
			using (Model.DbContextJulio db = new Model.DbContextJulio())
			{
				var aspnetuser_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@fecha_inicial", fecha_inicial);
				parameters.Add("@fecha_final", fecha_final);
				parameters.Add("@distribuidor_id", distribuidor_id);
				parameters.Add("@status_participante_id", status_participante_id);
				parameters.Add("@usuario_id", aspnetuser_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_reporte_participantes]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("Actividad")]
		public string actividad(string fecha_inicial, string fecha_final, decimal distribuidor_id)
		{
			using (Model.DbContextJulio db = new Model.DbContextJulio())
			{
				var aspnetuser_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@fecha_inicial", fecha_inicial);
				parameters.Add("@fecha_final", fecha_final);
				parameters.Add("@distribuidor_id", distribuidor_id);
				parameters.Add("@usuario_id", aspnetuser_id);
				DataSet setTables = db.GetDataSet("[dbo].[usp_reporte_actividad]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("cargaStatusParticipante")]
		public string cargaStatusParticipante()
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_status_participante]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("cargaStatusSeguimiento")]
		public string cargaStatusSeguimiento()
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				var usuario_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@usuario_id", usuario_id);
				DataSet setTables = db.GetDataSet("[dbo].[usp_consultar_status_seguimiento]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("Llamadas")]
		public string llamadas(string fecha_inicial, string fecha_final, decimal distribuidor_id, decimal status_participante_id, decimal status_seguimiento_id)
		{
			using (Model.DbContextJulio db = new Model.DbContextJulio())
			{
				var aspnetuser_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@fecha_inicial", fecha_inicial);
				parameters.Add("@fecha_final", fecha_final);
				parameters.Add("@distribuidor_id", distribuidor_id);
				parameters.Add("@status_participante_id", status_participante_id);
				parameters.Add("@status_llamada_id", status_seguimiento_id);
				parameters.Add("@usuario_id", aspnetuser_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_reporte_detalle_llamadas]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("Fraude")]
		public string repFraude(string fecha, int tipo_reporte)
		{
			using (Model.DbContextJulio db = new Model.DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@selected_date", Convert.ToDateTime(fecha));
				parameters.Add("@report_type", tipo_reporte);
				parameters.Add("@user_id", User.Identity.GetUserId());
				DataSet setTables = db.GetDataSet("[dbo].[usp_reporte_fraude]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		/*******/
		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("Operativo")]
		public string operativo(int tipo_reporte_id, string fecha_inicial, string fecha_final)
		{
			using (Model.DbContextJulio db = new Model.DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@tipo_reporte_id", tipo_reporte_id);
				parameters.Add("@fecha_inicial", fecha_inicial);
				parameters.Add("@fecha_final", fecha_final);								
				parameters.Add("@usuario_id", User.Identity.GetUserId());
				DataSet setTables = db.GetDataSet("[dbo].[usp_reportes_operativos]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		[AuditFilterApi, APIExceptionFilter]
		[HttpGet]
		[Route("Operativo_Detalle")]
		public string operativo_detalle(int tipo_reporte_id, string fecha_inicial, string fecha_final, int campo_id, decimal distribuidor_id)
		{
			using (Model.DbContextJulio db = new Model.DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@tipo_reporte_id", tipo_reporte_id);
				parameters.Add("@fecha_inicial", fecha_inicial);
				parameters.Add("@fecha_final", fecha_final);
				parameters.Add("@campo_id", campo_id);
				parameters.Add("@distribuidor_id", distribuidor_id);
				parameters.Add("@usuario_id", User.Identity.GetUserId());
				DataSet setTables = db.GetDataSet("[dbo].[usp_reportes_operativos_detalle]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}		
	}
}