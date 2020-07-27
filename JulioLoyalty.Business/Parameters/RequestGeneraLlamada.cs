using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequestGeneraLlamada
    {
        public string id { get; set; }
        public string nombre_campania { get; set; }
        public string campania_llamada { get; set; }
        public string query { get; set; }
        public string script { get; set; }
        public List<Lista> miembros { get; set; }
        public List<Usuarios> usuarios { get; set; }
        public class Lista
        {
            public string hidden { get; set; }
            public string clave { get; set; }
            public string nombre { get; set; }    
            public string usuario_id { get; set; }     
        }

        public class Usuarios
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string Llamada { get; set; }
        }
    }
}