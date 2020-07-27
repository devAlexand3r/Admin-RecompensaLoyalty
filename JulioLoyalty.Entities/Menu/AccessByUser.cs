using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Menu
{
    public class AccessByUser
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public int IdSubMenu { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual SubMenu SubMenu { get; set; }
    }
}
