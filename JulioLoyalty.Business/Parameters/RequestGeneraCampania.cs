using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
	public class RequestGeneraCampania
	{
        public string id { get; set; }
		public string nombre_lista { get; set; }
		public string permiso_recordatorio { get; set; }
		public string nombre_campania { get; set; }
		public string campania_pendiente { get; set; }
		public string asunto { get; set; }
		public string correo_responder { get; set; }
		public string nombre_responder { get; set; }
		public string UserName { get; set; }
		public string query { get; set; }
		public List<Lista> miembros { get; set; }

		public class Lista
		{
			public string hidden { get; set; }
			public string clave { get; set; }
			public string nombre { get; set; }
			public string correo { get; set; }
			public string fecha { get; set; }
			public string ciudad { get; set; }
			public string estado { get; set; }
			public string tienda { get; set; }
			public string status { get; set; }
            public string telefono { get; set; }
            public string nivel { get; set; }
        }
	}
}
