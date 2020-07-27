using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada
{
    public class cBase
    {
        public decimal id { get; set; }
        public string descripcion { get; set; }
        public string descripcion_larga { get; set; }
        public string script { get; set; }
        public class cTotal_Llamadas
        {
            public int total_llamadas { get; set; }
            public int total_llamadas_cerradas { get; set; }
        }
    }
}