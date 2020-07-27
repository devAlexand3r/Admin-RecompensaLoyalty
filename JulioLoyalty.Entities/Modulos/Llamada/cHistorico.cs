using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada
{
    public class cHistorico
    {
        public decimal id { get; set; }
        public string clave { get; set; }
        public string nombre_completo { get; set; }
        public string comentarios { get; set; }
        public List<cDetalles> detalles { get; set; }        
    }

    public class cDetalles
    {
        public decimal id { get; set; }
        public decimal status_id { get; set; }
        public string clave { get; set; }
        public string nombre_completo { get; set; }
        public DateTime fecha_alta { get; set; }
        public string comentarios { get; set; }
        public string status { get; set; }
    }
}
