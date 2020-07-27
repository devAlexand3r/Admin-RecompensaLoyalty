using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequestComments
    {
        public decimal participante_id { get; set; }
        public int status_id { get; set; }
        public string comentarios { get; set; }
        public string userName { get; set; }
    }
}
