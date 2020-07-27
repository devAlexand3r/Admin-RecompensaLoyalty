using JulioLoyalty.Entities.Mantenimiento;
using JulioLoyalty.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.MCatalogos
{
	public class ConsultaCatalagos
	{
		/// <summary>
		/// Obtener status participantes
		/// </summary>
		/// <returns></returns>
		public List<Lista> StatusParticipante()
		{
			List<Lista> lista = new List<Lista>();
			using (DbContextJulio db = new DbContextJulio())
			{
				var status = db.status_participante.Select(s => new
				{
					s.id,
					s.descripcion,
					s.descripcion_larga
				}).ToList();

				foreach (var statu in status)
				{
					lista.Add(new Lista() { id = statu.id, descripcion = statu.descripcion, descripcion_larga = statu.descripcion_larga });
				}
			}
			return lista;
		}

		/// <summary>
		/// Obtener status participante disponible
		/// </summary>
		/// <returns></returns>
		public List<Lista> StatusParticipante(decimal participante_id)
		{
			List<Lista> lista = new List<Lista>();
			using (DbContextJulio db = new DbContextJulio())
			{
				var status_participante_id = db.participante.Where(w => w.id == participante_id).FirstOrDefault().status_participante_id;
				var status = db.status_participante.Where(w => w.id == status_participante_id).Select(s => new
				{
					s.id,
					s.descripcion,
					s.descripcion_larga
				}).ToList();

				foreach (var statu in status)
				{
					lista.Add(new Lista() { id = statu.id, descripcion = statu.descripcion, descripcion_larga = statu.descripcion_larga });
				}
			}
			return lista;
		}

		/// <summary>
		/// Obtener distribuidores disponibles
		/// </summary>
		/// <returns></returns>
		public List<Lista> Distribuidores()
		{
			List<Lista> lista = new List<Lista>();
			using (DbContextJulio db = new DbContextJulio())
			{
				var distribuidores = db.distribuidor.Where(w => w.fecha_baja == null).Select(s => new
				{
					s.id,
					s.clave,
					s.descripcion,
					s.descripcion_larga
				}).ToList().OrderBy(s => s.descripcion);
				foreach (var distribuidor in distribuidores)
				{
					lista.Add(new Lista() { id = distribuidor.id, clave = distribuidor.clave, descripcion = distribuidor.descripcion, descripcion_larga = distribuidor.descripcion_larga });
				}
			}
			return lista;
		}

		/// <summary>
		/// Obtener distribuidores disponibles por usuario
		/// </summary>
		/// <returns></returns>
		public List<Lista> Distribuidores(string usuario)
		{
			List<Lista> lista = new List<Lista>();
			using (DbContextJulio db = new DbContextJulio())
			{
				var userId = db.AspNetUsers.Where(s => s.UserName == usuario).FirstOrDefault().Id;
				var distribuidores = db.AspNetUsers_Distribuidor.Where(w => w.IdUser == userId).Select(s => new
				{
					s.distribuidor.id,
					s.distribuidor.clave,
					s.distribuidor.descripcion,
					s.distribuidor.descripcion_larga,
					s.distribuidor.fecha_baja
				}).Where(s => s.fecha_baja == null).ToList().OrderBy(s => s.descripcion);
				foreach (var distribuidor in distribuidores)
				{
					lista.Add(new Lista() { id = distribuidor.id, clave = distribuidor.clave, descripcion = distribuidor.descripcion, descripcion_larga = distribuidor.descripcion_larga });
				}
			}
			return lista;
		}
	}
}
