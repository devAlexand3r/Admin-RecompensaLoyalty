using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequestActiva
    {
        public int participante_id { get; set; }
        public string NoTarjeta { get; set; }
        public string NoTienda { get; set; }
        public string NoTicket { get; set; }
    }
}
