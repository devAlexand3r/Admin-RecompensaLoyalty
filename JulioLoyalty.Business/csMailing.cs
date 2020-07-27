using JulioLoyalty.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JulioLoyalty.Business
{
	public class csMailing
	{
		private Mailing _mailing;
		public csMailing()
		{
		}
		public static DatosMailing Datos(decimal programa_id)
		{
			using (Model.DbContextJulio entities = new Model.DbContextJulio())
			{
				var _mailing = (from p in entities.pais
								where p.id == programa_id
								select new DatosMailing
								{
									ProgramaID = p.id,
									Programa = p.descripcion,
									FromMail = p.correo_electronico,
									FromName = p.remitente,
									ServidorSMTP = p.servidor_smtp,
									ServidorPOP = p.servidor_pop,
									UsuarioMail = p.usuario_correo,
									PasswordMail = p.password_correo
								}).FirstOrDefault();
				return _mailing;
			}
		}
		public static DatosMailing Datos(string clave)
		{
			using (Model.DbContextJulio entities = new Model.DbContextJulio())
			{
				var _mailing = (from p in entities.pais
								where p.clave == clave
								select new DatosMailing
								{
									ProgramaID = p.id,
									Programa = p.descripcion,
									FromMail = p.correo_electronico,
									FromName = p.remitente,
									ServidorSMTP = p.servidor_smtp,
									ServidorPOP = p.servidor_pop,
									UsuarioMail = p.usuario_correo,
									PasswordMail = p.password_correo
								}).FirstOrDefault();
				return _mailing;
			}
		}
		public List<Mailing> Busqueda(string busqueda, string pais)
		{
			busqueda = busqueda.ToLower();
			List<Mailing> list_mailing = new List<Mailing>();
			using (Model.DbContextJulio entities = new Model.DbContextJulio())
			{
				var mail = from b in entities.mailing
						   where b.ToMail.ToLower().Contains(busqueda) ||
								 b.ToName.ToLower().Contains(busqueda) ||
								 b.Subject.ToLower().Contains(busqueda) &&
								 b.pais == pais
						   select b;
				foreach (var _m in mail)
				{
					_mailing = new Mailing();
					_mailing.ToMail = _m.ToMail;
					_mailing.ToName = _m.ToName;
					_mailing.Subject = _m.Subject;
					list_mailing.Add(_mailing);
				}
				return list_mailing;
			}
		}
	}
}
