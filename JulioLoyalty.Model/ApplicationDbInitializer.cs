
using JulioLoyalty.Entities;
using JulioLoyalty.Entities.Menu;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Model
{
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {

            InitializeIdentityForEF(context);
            base.Seed(context);
        }
        private static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            InitializeSecurity(db);
            InitilizeCatalogs(db);            
        }
        private static void InitializeSecurity(ApplicationDbContext db)
        {
            const string firstName = "Alejandro";
            const string middleName = "Jimenez";
            const string lastName = "Gonzalez";
            const string userName = "desarrollo";
            const string password = "loyalty";
            const string userEmail = "alejandro.jimenez.26@outlook.com";

            string[] roles = { "Administrador" , "Distribuidor", "Participante" };
            string[] routes = { "~/Home", "~/Home", "~/Home"};
            string[] roleNameDescriptions = {
                "Acceso total a la aplicación",
                "Acceso exclusivo para distribuidores",
                "Acceso exclusivo para participantes"
            };
            
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            for (int i = 0; i < roles.Length; i++)
            {
                var role = roleManager.FindByName(roles[i]);
                if (role == null)
                {
                    role = new ApplicationRole() { Name = roles[i], Description = roleNameDescriptions[i], InitialPageUrl = routes[i] };
                    var rolResult = roleManager.Create(role);
                }
            }

            var user = UserManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser() { UserName = userName, Email = userEmail, Profiles = new Profiles() { FirstName = firstName, MiddleName = middleName, LastName = lastName, Age = 25 } };
                var userResult = UserManager.Create(user, password);
                userResult = UserManager.SetLockoutEnabled(user.Id, false);
            }

            var rolesForUser = UserManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(roles[0]))
            {
                var result = UserManager.AddToRole(user.Id, roles[0]);
            }
        }

        private static void InitilizeCatalogs(ApplicationDbContext db)
        {
            const string userName = "desarrollo";

            #region --> Menus
            List<Entities.Menu.Menu> menus = new List<Entities.Menu.Menu>();
            menus.Add(new Entities.Menu.Menu { Name = "Control de acceso", Index = 1, CreationDate = DateTime.Now, });
            menus.Add(new Entities.Menu.Menu { Name = "Participantes", Index = 2, CreationDate = DateTime.Now, });
            menus.Add(new Entities.Menu.Menu { Name = "Llamadas", Index = 3, CreationDate = DateTime.Now, });
            menus.Add(new Entities.Menu.Menu { Name = "Canjes", Index = 4, CreationDate = DateTime.Now, });
            menus.Add(new Entities.Menu.Menu { Name = "Reportes", Index = 5, CreationDate = DateTime.Now, });
            #endregion

            #region --> SubMenus
            List<SubMenu> submenus = new List<SubMenu>();
            submenus.Add(new SubMenu() { IdMenu = 1, Name = "Usuarios", ControllerName = "Configuration", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 1, Name = "Menú", ControllerName = "Configuration", ActionName = "Menu", IsActive = true, CreationDate = DateTime.Now });
            //submenus.Add(new SubMenu() { IdMenu = 1, Name = "Funciones", ControllerName = "Configuration", ActionName = "Function", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 1, Name = "Roles", ControllerName = "Configuration", ActionName = "Roles", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 1, Name = "Registro de participantes", ControllerName = "Configuration", ActionName = "Register", IsActive = true, CreationDate = DateTime.Now });

            submenus.Add(new SubMenu() { IdMenu = 2, Name = "Datos generales", ControllerName = "Operation", ActionName = "General", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 2, Name = "Consulta transacciones", ControllerName = "Operation", ActionName = "Transaction", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 2, Name = "Agergar transaccion", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 2, Name = "Canjes", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 2, Name = "Accesos", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 2, Name = "Mecánica", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });

            submenus.Add(new SubMenu() { IdMenu = 3, Name = "Captura", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 3, Name = "Seguimiento", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 3, Name = "Históricon", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 3, Name = "Envio de mensajes", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });

            submenus.Add(new SubMenu() { IdMenu = 4, Name = "Centro de canjes", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });

            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Participantes", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Puntos", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Llamadas", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Canjes", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Perfiles", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Pasivos", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Cubo de participantes", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Cubo de actividad", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Cubo de puntos", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Marcas", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Productos", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            submenus.Add(new SubMenu() { IdMenu = 5, Name = "Productos globales", ControllerName = "Home", ActionName = "Index", IsActive = true, CreationDate = DateTime.Now });
            #endregion

            #region --> Access by user
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindByName(userName);

            List<AccessByUser> acces = new List<AccessByUser>();
            if (user != null)
            {
                acces.Add(new AccessByUser() { IdSubMenu = 1, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 2, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 3, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 4, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 5, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 6, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 7, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 8, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 9, IdUser = user.Id });
                acces.Add(new AccessByUser() { IdSubMenu = 10, IdUser = user.Id });
            }

            #endregion

            db.Menu.AddRange(menus);
            db.SubMenu.AddRange(submenus);           
            db.SaveChanges();
            db.AccessByUser.AddRange(acces);
            db.SaveChanges();
        }
    }
}
