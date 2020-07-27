using JulioLoyalty.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Business.Llamada;

namespace JulioLoyalty.Business.Funciones
{
	public class envioMail
	{
		private string dominio = ConfigurationManager.AppSettings["dominio"].ToString();
		public void envioMailParticipante(string plantilla, string asunto, string nombre, string email)
		{
			plantilla = System.Web.Hosting.HostingEnvironment.MapPath(plantilla);
			string html = string.Empty;
			using (StreamReader sr = new StreamReader(plantilla))
			{
				html = sr.ReadToEnd();
			}
			html = html.Replace("@DOMINIO", dominio);
			csEnvioMail envio = new csEnvioMail(ConfigurationManager.AppSettings["programa"].ToString());
			DatosClienteMailing datos = new DatosClienteMailing();
			List<DatosClienteMailing> destinatario = new List<DatosClienteMailing>();
			List<string> cc = new List<string>();
			List<string> bcc = new List<string>();
			datos.ToMail = email;
			if (!string.IsNullOrEmpty(nombre))
				datos.ToName = nombre;
			else
				datos.ToName = email;
			destinatario.Add(datos);
			envio.Enviar(destinatario, asunto, html, cc, bcc);
		}

		public void envioMailMensaje(string plantilla, string asunto, string nombre, string email, string mensaje)
		{
			plantilla = System.Web.Hosting.HostingEnvironment.MapPath(plantilla);
			string html = string.Empty;
			using (StreamReader sr = new StreamReader(plantilla))
			{
				html = sr.ReadToEnd();
			}
			html = html.Replace("@DOMINIO", dominio);
			html = html.Replace("@NOMBRE", nombre);
			html = html.Replace("@COMENTARIOS", mensaje);
			csEnvioMail envio = new csEnvioMail(ConfigurationManager.AppSettings["programa"].ToString());
			DatosClienteMailing datos = new DatosClienteMailing();
			List<DatosClienteMailing> destinatario = new List<DatosClienteMailing>();
			List<string> cc = new List<string>();
			List<string> bcc = new List<string>();
			datos.ToMail = email;
			if (!string.IsNullOrEmpty(nombre))
				datos.ToName = nombre;
			else
				datos.ToName = email;
			destinatario.Add(datos);
			envio.Enviar(destinatario, asunto, html, cc, bcc);
		}

		public void envioMailSeguimientoLlamada(string plantilla, string asunto, string mensaje)
		{
			plantilla = System.Web.Hosting.HostingEnvironment.MapPath(plantilla);
			string html = string.Empty;
			using (StreamReader sr = new StreamReader(plantilla))
			{
				html = sr.ReadToEnd();
			}
			html = html.Replace("@MENSAJE", mensaje);
			csEnvioMail envio = new csEnvioMail(ConfigurationManager.AppSettings["programa"].ToString());
			DatosClienteMailing datos = new DatosClienteMailing();
			List<DatosClienteMailing> destinatario = new List<DatosClienteMailing>();
			List<string> cc = new List<string>();
			List<string> bcc = new List<string>();
			datos.ToMail = envio.servidor.FromMail;
			datos.ToName = envio.servidor.FromName;
			destinatario.Add(datos);
			envio.Enviar(destinatario, asunto, html, cc, bcc);
		}
	}
}