using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Campaign
{
   public class cUsuariosLlamadas
    {
        public class UsuariosLlamadas
        {
            public string id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string Llamada { get; set; }
        }
        public List<UsuariosLlamadas> lstUsuariosLlamadas { get; set; }
    }
}