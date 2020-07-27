
using JulioLoyalty.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.RenderMenu
{
    public interface IRepositoryMenu
    {
        List<Menu> GetMenuList();
        List<SubMenu> GetSubMenuList();
    }
}
