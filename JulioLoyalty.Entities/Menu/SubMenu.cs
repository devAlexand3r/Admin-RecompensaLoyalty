using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Menu
{
    public class SubMenu
    {
        public int Id { get; set; }
        public int IdMenu { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual List<AccessByUser> AccessByUser { get; set; }
    }
}
