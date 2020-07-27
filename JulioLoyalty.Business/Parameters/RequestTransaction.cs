using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequestTransaction
    {
        public decimal Participante_id { get; set; }
        public string NoTarjeta { get; set; }
        public decimal Tipo_transaccion_id { get; set; }
        public int Puntos { get; set; }
        public string Fecha_transaccion { get; set; }

        public string userName { get; set; }
        public string comentarios { get; set; }
    }
}
