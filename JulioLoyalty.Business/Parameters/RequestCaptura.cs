using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
	public class RequestCaptura
	{
		public decimal Participante_Id { get; set; }
		public string Participante { get; set; }
		public decimal Distribuidor_Id { get; set; }
		public string Persona { get; set; }
		public string Lada { get; set; }
		public string Telefono { get; set; }
		public string Comentarios { get; set; }
		public string CadenaTipoLlamada { get; set; }
		public decimal Status_Seguimiento_Id { get; set; }
		public decimal IDlamada { get; set; }
		public string UserName { get; set; }
	}
}