using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequestRepOtUnif
    {
        public int participante_id { get; set; }
        public string tranferencia { get; set; }
        public string NoTarjeta { get; set; }
        public string NuevaTarjeta { get; set; }
    }
}
