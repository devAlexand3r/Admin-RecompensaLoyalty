using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequesUser
    {
        public string Key { get; set; }
        //public string FistName { get; set; }
        //public string MiddleName { get; set; }
        //public string LastName { get; set; }
        //public int Age { get; set; }
        //public string Email { get; set; }

        public string Nombre { get; set; }
        public string Ape_paterno { get; set; }
        public string Ape_materno { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }

        public int Age { get; set; }

        public string Distribuidor { get; set; }
    }
}
