using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Entities;
using JulioLoyalty.Model;
using JulioLoyalty.Model.EntitiesModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using Newtonsoft.Json;
using System.IO;

namespace JulioLoyalty.Business.Llamada
{
	public class Llamada : IRepositoryLlamada
	{
		private decimal llamada_id;
		private decimal llamada_seguimiento_id;
		private string descripcion_llamada;
		private decimal status_seguimiento_id;
		private string status_seguimiento;
		private string usuario_id;
		private decimal escalamiento_id;
		private string escalamiento;
		private string participante;
		private string clave;
		private string distribuidor;
		private List<string> lista_tipo_llamada;
		private List<string> lista_tipo_llamada_salida;
		private ResultJson result = new ResultJson();
		public Llamada() { }
		public decimal IDlamada
		{
			get
			{
				return llamada_id;
			}
			set
			{
				llamada_id = value;
			}
		}
		public decimal IDlamada_seguimiento
		{
			get
			{
				return llamada_seguimiento_id;
			}
			set
			{
				llamada_seguimiento_id = value;
			}
		}
		public string Descripcion
		{
			get
			{
				return descripcion_llamada;
			}
			set
			{
				descripcion_llamada = value;
			}
		}
		public decimal Status_seguimiento_id
		{
			get
			{
				return status_seguimiento_id;
			}
			set
			{
				status_seguimiento_id = value;
			}
		}
		public string Status_segumiento
		{
			get
			{
				return status_seguimiento;
			}
			set
			{
				status_seguimiento = value;
			}
		}
		public decimal Escalamiento_id
		{
			get
			{
				return escalamiento_id;
			}
			set
			{
				escalamiento_id = value;
			}
		}
		public string Escalamiento
		{
			get
			{
				return escalamiento;
			}
			set
			{
				escalamiento = value;
			}
		}
		public void AgregaTipoLlamada(string tipo_llamada)
		{
			if (lista_tipo_llamada == null)
			{
				lista_tipo_llamada = new List<string>();
				lista_tipo_llamada.Add(tipo_llamada);
			}
			else
			{
				lista_tipo_llamada.Add(tipo_llamada);
			}
		}
		public void AgregaTipoLlamadaSalida(string tipo_llamada_salida)
		{
			if (lista_tipo_llamada_salida == null)
			{
				lista_tipo_llamada_salida = new List<string>();
				lista_tipo_llamada_salida.Add(tipo_llamada_salida);
			}
			else
			{
				lista_tipo_llamada_salida.Add(tipo_llamada_salida);
			}
		}
		public int ConsultaCantidadTipoLlamada
		{
			get
			{
				if (lista_tipo_llamada == null)
				{
					return 0;
				}
				else
				{
					return lista_tipo_llamada.Count;
				}
			}
		}
		public ResultJson Envio(RequestEnvio envio)
		{
			try
			{
				using (DbContextJulio db = new DbContextJulio())
				{
					var _userEnvio = db.AspNetUsers.Where(s => s.UserName == envio.UserName).FirstOrDefault();
					// Inserta en bitacora_envios
					bitacora_envios bitacora = new bitacora_envios()
					{
						participante_id = envio.Participante_Id,
						correo_electronico = envio.Correo,
						asunto = envio.Asunto,
						mensaje = envio.Mensaje,
						fecha = DateTime.Now,
						usuario_id = Guid.Parse(_userEnvio.Id),
					};
					db.bitacora_envios.Add(bitacora);
					db.SaveChanges();
					// Envia correo al participante
					Funciones.envioMail envioCorreo = new Funciones.envioMail();
					envioCorreo.envioMailMensaje("~/Plantillas/envioMensaje.html", envio.Asunto, envio.Participante, envio.Correo, envio.Mensaje);
					result.Success = true;
					result.Message = "Mensaje enviado.";
					return result;
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = "Ocurrió un error al enviar el correo. Intente nuevamente";
				result.InnerException = $"{ex.Message}";
				return result;
			}
		}

		public string Historico(decimal? participante_id)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@participante_id", participante_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_historial_llamadas]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		public string ConsultaLlamada(decimal? llamada_id)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@llamada_id", llamada_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_consulta_llamada]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		public string HistoricoDetalle(decimal? llamada_id)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@llamada_id", llamada_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_historial_llamadas_detalle]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		private DataTable ConsultaCategoriaTipoLlamada()
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				DataSet setTables = db.GetDataSet("[dbo].[sp_consulta_categoria_tipo_llamada]", CommandType.StoredProcedure, parameters);
				return setTables.Tables[0];
			}
		}

		private DataTable ConsultaTipoLlamada(decimal categoria_id)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@categoria_id", categoria_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_consulta_tipo_llamada]", CommandType.StoredProcedure, parameters);
				return setTables.Tables[0];
			}
		}

		public string CargaStatusSeguimientoAbierto()
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				DataSet setTables = db.GetDataSet("[dbo].[usp_consulta_status_seguimiento_abierto]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		public string CargaTelefono(decimal? participante_id)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@participante_id", participante_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_consulta_telefono_participante]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		public string CargaTipoLlamada()
		{
			List<Entities.Llamada> lstLlamada = new List<Entities.Llamada>();
			var lstCategoriaTipoLlamada = ConsultaCategoriaTipoLlamada();
			foreach (DataRow item in lstCategoriaTipoLlamada.Rows)
			{
				decimal categoria_tipo_llamada_id = decimal.Parse(item["id"].ToString());
				string clave_categoria_tipo_llamada = item["clave"].ToString();
				string descripcion_categoria_tipo_llamada = item["descripcion"].ToString();
				var lstTipoLlamada = ConsultaTipoLlamada(categoria_tipo_llamada_id);
				foreach (DataRow itemTipoLlamada in lstTipoLlamada.Rows)
				{
					lstLlamada.Add(new Entities.Llamada(categoria_tipo_llamada_id,
														clave_categoria_tipo_llamada,
														descripcion_categoria_tipo_llamada,
														decimal.Parse(itemTipoLlamada["id"].ToString()),
														itemTipoLlamada["descripcion"].ToString()));
				}
			}
			//return lstLlamada;
			return JsonConvert.SerializeObject(lstLlamada);
		}

		public ResultJson Captura(RequestCaptura captura)
		{
			try
			{
				status_seguimiento_id = captura.Status_Seguimiento_Id;
				string[] lstCadena = captura.CadenaTipoLlamada.Split(',');
				foreach (string item in lstCadena)
				{
					string[] lstSepara = item.Split(':');
					string clave_categoria_llamada = lstSepara[0];
					string tipo_llamada_id = lstSepara[1];
					AgregaTipoLlamada(tipo_llamada_id);
					if (clave_categoria_llamada == "SAL")
						AgregaTipoLlamadaSalida(tipo_llamada_id);
				}
				GuardaLlamada(captura.Participante_Id, captura.Participante, captura.Status_Seguimiento_Id, captura.Distribuidor_Id, captura.Persona, captura.Telefono, captura.Comentarios, captura.UserName);				
				captura.IDlamada = IDlamada;
				ActualizaStatusSeguimiento(captura.IDlamada, "CR"); // Checa y actualiza si cierra la llamada si solo tiene llamadas de salida
				result.Success = true;
				result.Message = "Información almacenada satisfactoriamente. Número de caso : " + captura.IDlamada;
				return result;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = "Ocurrió un error al guardar la llamada";
				result.InnerException = $"{ex.Message}";
				return result;
			}
		}

		private void GuardaLlamada(decimal participante_id, string participante, decimal status_seguimiento_id, decimal distribuidor_id, string nombre_llama, string telefono, string descripcion, string username)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				var _user = db.AspNetUsers.Where(s => s.UserName == username).FirstOrDefault();
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@participante_id", participante_id);
				parameters.Add("@participante", participante);
				parameters.Add("@status_seguimiento_id", status_seguimiento_id);
				parameters.Add("@distribuidor_id", distribuidor_id);
				parameters.Add("@nombre_llama", nombre_llama);
				parameters.Add("@telefono", telefono);
				parameters.Add("@descripcion", descripcion);
				parameters.Add("@usuario_id", _user.Id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_guarda_llamada]", CommandType.StoredProcedure, parameters);
				DataTable dtLlamada = setTables.Tables[0];
				foreach (DataRow row in dtLlamada.Rows)
				{
					foreach (var item in row.ItemArray)
					{
						llamada_id = decimal.Parse(item.ToString());
						descripcion_llamada = descripcion;
						usuario_id = _user.Id;
					}
				}
				lista_tipo_llamada.ForEach(GuardaTipoLlamada);
			}
		}

		private void GuardaTipoLlamada(string tipollamada)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@llamada_id", llamada_id);
				parameters.Add("@tipo_llamada_id", tipollamada);
				db.GetDataSet("[dbo].[sp_inserta_llamada_tipo_llamada]", CommandType.StoredProcedure, parameters);
				DataTable dtClave = ObtieneClaveTipoLlamada(tipollamada);
				if (dtClave.Rows.Count > 0)
				{
					DataRow drClave = dtClave.Rows[0];
					if (drClave["clave"].ToString().ToUpper() != "CANJE" && drClave["clave_categoria"].ToString().ToUpper() != "SAL")
					{
						DataTable dtIdllamada;
						if (drClave["clave_categoria"].ToString().ToUpper() != "COMENTARIO")
							dtIdllamada = ObtieneIdTipoLlamada("OPCALLC");
						else
							dtIdllamada = ObtieneIdTipoLlamada(drClave["clave"].ToString().ToUpper());
						if (dtIdllamada.Rows.Count > 0)
						{
							DataRow drIdllamada = dtIdllamada.Rows[0];
							GuardaLlamadaSeguimiento(decimal.Parse(drIdllamada["id"].ToString()));
						}
					}					
				}
			}
		}

		protected DataTable ObtieneClaveTipoLlamada(string tipo_llamada)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@tipo_llamada", tipo_llamada);
				DataSet setTables = db.GetDataSet("[dbo].[Obtiene_Clave_Tipo_Llamada]", CommandType.StoredProcedure, parameters);
				DataTable dtLlamada = setTables.Tables[0];
				return dtLlamada;
			}
		}

		protected DataTable ObtieneIdTipoLlamada(string clave)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@clave", clave);
				DataSet setTables = db.GetDataSet("[dbo].[ObtieneIdTipo_llamada]", CommandType.StoredProcedure, parameters);
				DataTable dtLlamada = setTables.Tables[0];
				return dtLlamada;
			}
		}

		protected void GuardaLlamadaSeguimiento(decimal tipollamada)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@llamada_id", llamada_id);
				parameters.Add("@observacion", descripcion_llamada);
				parameters.Add("@tipo_llamada_id", tipollamada);
				parameters.Add("@status_seguimiento_id", status_seguimiento_id);
				parameters.Add("@aspnet_UserId", usuario_id);
				DataSet setTables = db.GetDataSet("[dbo].[sp_guarda_llamada_seguimiento]", CommandType.StoredProcedure, parameters);
				DataTable dtLlamada = setTables.Tables[0];
				foreach (DataRow row in dtLlamada.Rows)
				{
					foreach (var item in row.ItemArray)
					{
						llamada_seguimiento_id = decimal.Parse(item.ToString());
					}
				}
			}
		}

		protected bool ActualizaStatusSeguimiento(decimal? llamada_id, string status)
		{
			try
			{
				using (DbContextJulio db = new DbContextJulio())
				{				
					Dictionary<string, object> parameters = new Dictionary<string, object>();
					parameters.Add("@llamada_id", llamada_id);
					parameters.Add("@status", status);
					db.GetDataSet("[dbo].[sp_actualiza_llamada_status_seguimiento_cerrado]", CommandType.StoredProcedure, parameters);
					return true;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public string Seguimiento(decimal? participante_id)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@participante_id", participante_id);
				DataSet setTables = db.GetDataSet("[dbo].[Obtiene_Seguimiento_Llamadas]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		public string SeguimientoDetalle(decimal? id)
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@id", id);
				DataSet setTables = db.GetDataSet("[dbo].[Obtiene_Seguimiento_Llamadas_Detalle]", CommandType.StoredProcedure, parameters);
				return JsonConvert.SerializeObject(setTables.Tables[0]);
			}
		}

		public ResultJson Seguimiento(RequestSeguimiento seguimiento)
		{
			try
			{
				using (DbContextJulio db = new DbContextJulio())
				{
					var _user = db.AspNetUsers.Where(s => s.UserName == seguimiento.UserName).FirstOrDefault();
					var _distribuidor = db.distribuidor.Where(s => s.id == seguimiento.Distribuidor_id).FirstOrDefault();
					llamada_id = seguimiento.Llamada_id;
					participante = seguimiento.Participante;
					clave = seguimiento.Clave;
					distribuidor = _distribuidor.descripcion;
					descripcion_llamada = seguimiento.Comentarios;
					usuario_id = _user.Id;
					status_seguimiento_id = seguimiento.Status_id;
					status_seguimiento = seguimiento.Status;
					escalamiento_id = seguimiento.Escalamiento_id;
					escalamiento = seguimiento.Escalamiento;
					GuardaLlamadaSeguimiento(seguimiento.Escalamiento_id);
					/* Obtiene la clave de status_segumiento */
					DataTable dtClaveStatusSeguimiento = ObtieneClaveStatusSeguimiento();
					if (dtClaveStatusSeguimiento.Rows.Count > 0)
					{
						DataRow drClaveStatusSeguimiento = dtClaveStatusSeguimiento.Rows[0];
						string clave_status_seguimiento = drClaveStatusSeguimiento["clave"].ToString().ToUpper();
						if (clave_status_seguimiento != "CR" && clave_status_seguimiento != "CN") // si el status es Cerrado Resuelto y Cerrado no Resuelto no manda mail
						{
							/* Envio Mail */
							envioMailSeguimiento("~/plantillas/aviso_llamada.html", "Seguimiento de llamada Julio Loyalty");
						}
					}
				}
				result.Success = true;
				result.Message = "Seguimiento de llamada guardada satisfactoriamente.";
				return result;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = "Ocurrió un error al guardar el seguimiento";
				result.InnerException = $"{ex.Message}";
				return result;
			}
		}

		protected DataTable ObtieneClaveStatusSeguimiento()
		{
			using (DbContextJulio db = new DbContextJulio())
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("@id", status_seguimiento_id);
				DataSet setTables = db.GetDataSet("[dbo].[Obtiene_clave_status_seguimiento]", CommandType.StoredProcedure, parameters);
				DataTable dtLlamada = setTables.Tables[0];
				return dtLlamada;
			}
		}

		protected void envioMailSeguimiento(string plantilla, string asunto)
		{
			string _datos = "<table>" +
				"<tr><td><b>Nombre:</b></td><td>" + participante + "</td></tr>" +
				"<tr><td><b>Tarjeta:</b></td><td>" + clave + "</td></tr>" +
				"<tr><td><b>Tienda:</b></td><td>" + distribuidor + "</td></tr></table>" +
				"<p>Se gener&oacute; la siguiente llamada:</p>" +
				"<table width='900px'><tr><td colspan='3' align='center'><b>Detalle de llamada</b></td></tr>" +
				"<tr><td><b>Escalamiento:</b></td><td>" + escalamiento + "</td></tr>" +
				"<tr><td><b>Comentarios:</b></td><td>" + descripcion_llamada + "</td></tr>" +
				"<tr><td><b>Status:</b></td><td>" + status_seguimiento + "</td></tr>" +
				"</table>";
			// Envia correo del seguimiento
			Funciones.envioMail envioCorreo = new Funciones.envioMail();
			envioCorreo.envioMailSeguimientoLlamada(plantilla, asunto, _datos);
		}
	}
}