using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada
{
    public class cModeloPrincipal
    {
        public decimal Id { get; set; }
        public string Nombre { get; set; }
        public List<cBase> Status { get; set; }
        public List<cTelefonos> Telefonos { get; set; }
        public List<cCitas> Citas { get; set; }
        public List<cBase> Campañas { get; set; }
        public cBase.cTotal_Llamadas Count_Campania_Llamadas { get; set; }
        public decimal campaña_id { get; set; }
        public string Script { get; set; }
    }
}
