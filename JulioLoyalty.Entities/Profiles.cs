using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities
{
    /// Clase POCO para representar datos personalizados de un usuario a través de Identity
    public class Profiles
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        //public bool IsActive { get; set; } = false;

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
