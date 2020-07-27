using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada
{
    public class cCitas
    {
        public int Id { get; set; }
        public decimal participante_id { get; set; }
        public decimal participante_camp_id { get; set; }
        public DateTime fecha_agendada { get; set; }
        public string comentarios { get; set; }
        public string Nombre { get; set; }
        public int status_cita { get; set; }
    }
}
