using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Entities.Menu;
using JulioLoyalty.Model;

namespace JulioLoyalty.Business.RenderMenu
{
    public class MenuDefault : IRepositoryMenu
    {
        private string Key; // Id, Identity

        public MenuDefault(string key)
        {
            this.Key = key;
        }
        //Obtener, lista de menu por usuario
        public List<Menu> GetMenuList()
        {
            List<Menu> menu = new List<Menu>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var queryAccess = db.AccessByUser.Where(s => s.IdUser == Key && s.SubMenu.IsActive == true).ToList();
                foreach (var obj in queryAccess)
                {
                    var check = menu.Where(s => s.Id == obj.SubMenu.IdMenu).FirstOrDefault();
                    if (check == null)
                    {
                        var result = db.Menu.Where(s => s.Id == obj.SubMenu.IdMenu).FirstOrDefault();
                        menu.Add(result);
                    }
                }
            }
            return menu.OrderBy(s => s.Index).ToList();
        } 
        
        //Obtener, lista de submenu por usuario
        public List<SubMenu> GetSubMenuList()
        {
            List<SubMenu> subMenu = new List<SubMenu>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var queryAccess = db.AccessByUser.Where(s => s.IdUser == Key && s.SubMenu.IsActive == true).ToList();
                foreach (var obj in queryAccess)
                {
                    var check = subMenu.Where(s => s.Id == obj.IdSubMenu).FirstOrDefault();
                    if (check == null)
                    {
                        var result = db.SubMenu.Where(s => s.Id == obj.IdSubMenu).FirstOrDefault();
                        subMenu.Add(result);
                    }
                }
            }
            return subMenu;
        }
    }
}
