using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using JulioLoyalty.Entities;

namespace JulioLoyalty.Business
{
	public class csEnvioMail
	{
		public int ProgramaID
		{ get; set; }
		public string NombrePrograma
		{ get; set; }
		public string Remitente
		{ get; set; }
		public string CorreoElectronico
		{ get; set; }
		public DatosMailing servidor { get; set; }
		public csEnvioMail(int programa_id)
		{
			servidor = csMailing.Datos(programa_id);
		}
		public csEnvioMail(string clave)
		{
			servidor = csMailing.Datos(clave);
		}
		public RespuestaEnvio Enviar(List<DatosClienteMailing> para, string subject, string body, List<string> cc, List<string> bcc)
		{
			try
			{
				DatosClienteMailing datos = new DatosClienteMailing
				{
					FromMail = servidor.FromMail,
					FromName = servidor.FromName
				};
				List<DatosClienteMailing> to = new List<DatosClienteMailing>();
				List<DatosClienteMailing> tocc = new List<DatosClienteMailing>();
				List<DatosClienteMailing> tobcc = new List<DatosClienteMailing>();
				to = para.Select(e => new DatosClienteMailing { ToMail = e.ToMail, ToName = e.ToName }).ToList();
				if (cc != null && cc.Count > 0)
				{
					tocc = cc.Select(c => new DatosClienteMailing { ToMail = c }).ToList();
				}
				if (bcc != null && bcc.Count > 0)
				{
					tobcc = bcc.Select(b => new DatosClienteMailing { ToMail = b }).ToList();
				}
				_enviar(para, subject, body, cc, bcc);
				return new RespuestaEnvio("OK", "");
			}
			catch (Exception ex)
			{
				return new RespuestaEnvio("ERROR", ex.Message);
			}
		}
		private void _enviar(List<DatosClienteMailing> para, string subject, string body, List<string> cc, List<string> bcc)
		{
			SmtpClient srvMail = new SmtpClient();
			if (string.IsNullOrEmpty(srvMail.Host))
			{
				srvMail = new SmtpClient
				{
					Host = servidor.ServidorSMTP,
					Credentials = new System.Net.NetworkCredential
					{
						UserName = servidor.UsuarioMail,
						Password = servidor.PasswordMail
					}
				};
			}
			MailAddress from = new MailAddress(servidor.FromMail, servidor.FromName);
			MailMessage mensaje = new MailMessage();
			mensaje.From = from;
			foreach (DatosClienteMailing d in para)
			{
				mensaje.To.Add(new MailAddress(d.ToMail, d.ToName));
			}
			mensaje.Subject = subject;
			mensaje.Body = body;
			mensaje.IsBodyHtml = true;
			if (cc != null && cc.Count > 0)
			{
				foreach (string _cc in cc)
				{
					mensaje.CC.Add(new MailAddress(_cc));
				}
			}
			if (bcc != null && bcc.Count > 0)
			{
				foreach (string _bcc in bcc)
				{
					mensaje.Bcc.Add(new MailAddress(_bcc));
				}
			}
			try
			{
				srvMail.Send(mensaje);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
