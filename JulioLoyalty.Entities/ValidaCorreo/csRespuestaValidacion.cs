using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.ValidaCorreo
{
    public class csRespuestaValidacion
    {
        public decimal id { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string correo_electronico { get; set; }
        public string telefono_celular { get; set; }
        public DateTime? fecha_nacimiento { get; set; }

        public int status { get; set; }
        public string message { get; set; }
        public string errorException { get; set; }
        public string result { get; set; }
        public string reason { get; set; }
    }
}
