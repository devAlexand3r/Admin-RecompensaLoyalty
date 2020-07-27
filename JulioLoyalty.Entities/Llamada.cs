using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities
{
	public class Llamada
	{
		public decimal Categoria_Tipo_Llamada_Id { get; set; }
		public string Clave_Categoria_Tipo_Llamada { get; set; }
		public string Descripcion_Categoria_Tipo_Llamada { get; set; }
		public decimal Tipo_Llamada_Id { get; set; }
		public string Descripcion_Tipo_Llamada { get; set; }
		public Llamada() { }
		public Llamada(decimal categoria_tipo_llamada_id, string clave_categoria_tipo_llamada, string descripcion_categoria_tipo_llamada, decimal tipo_llamada_id, string descripcion_tipo_llamada)
			: this()
		{
			this.Categoria_Tipo_Llamada_Id = categoria_tipo_llamada_id;
			this.Clave_Categoria_Tipo_Llamada = clave_categoria_tipo_llamada.Trim();
			this.Descripcion_Categoria_Tipo_Llamada = descripcion_categoria_tipo_llamada.Trim();
			this.Tipo_Llamada_Id = tipo_llamada_id;
			this.Descripcion_Tipo_Llamada = descripcion_tipo_llamada.Trim();
		}
		public List<Escalamiento> Escalamiento { get; set; }
		public List<Status> Status { get; set; }
	}
	public class Escalamiento
	{
		public string Id { get; set; }
		public string Descripcion { get; set; }
	}
	public class Status
	{
		public string Id { get; set; }
		public string Descripcion { get; set; }
	}
}