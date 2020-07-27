using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada
{
    public class cParticipante
    {
        public decimal Id { get; set; }
        public string Nombre { get; set; }
        public string Ejecutivo { get; set; }
        public decimal campaña_id { get; set; }
    }
}
