using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business
{
    public class ResultJson
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Transacción exitosa";
        public string Exception { get; set; }
        public string InnerException { get; set; }

        public string jsonObject { get; set; }

        public byte? acumula { get; set; }
        public string status { get; set; }
        public string acumula_mensaje { get; set; }

    }
}
