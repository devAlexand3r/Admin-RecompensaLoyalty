using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada
{
    public class cBusqueda
    {
        public decimal participante_id { get; set; }
        public decimal participante_camp_id { get; set; }
        public string nombre_completo { get; set; }
        public DateTime? fecha_ultima { get; set; }
        public DateTime? fecha_siguiente { get; set; }
        public DateTime? fecha_agendada { get; set; }
        public int contador { get; set; }
        public string nombre_campaña { get; set; }
        public string comentarios { get; set; }
        public int resultado_total { get; set; }
    }
}
