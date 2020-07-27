using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
	public class RequestSeguimiento
	{
		public decimal Llamada_id { get; set; }
		public decimal Participante_id { get; set; }
		public string Participante { get; set; }
		public string Clave { get; set; }
		public decimal Distribuidor_id { get; set; }
		public string Distribuidor { get; set; }
		public string Comentarios { get; set; }
		public decimal Status_id { get; set; }
		public string Status { get; set; }
		public decimal Escalamiento_id { get; set; }
		public string Escalamiento { get; set; }
		public string UserName { get; set; }
	}
}