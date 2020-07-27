using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
	public class RequestEnvio
	{
		public decimal Participante_Id { get; set; }
		public string Participante { get; set; }
		public string Correo { get; set; }
		public string Asunto { get; set; }
		public string Mensaje { get; set; }
		public string UserName { get; set; }
	}
}
