using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Entities;
using JulioLoyalty.Entities.Menu;
using JulioLoyalty.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JulioLoyalty.Business.Services
{
    public class DefaultService : IRepositoryService
    {
        private ResultJson result = new ResultJson();
        public DefaultService() { }

        public ResultJson AddOrUpdateAction(RequestAction request)
        {           
            if (string.IsNullOrEmpty(request.Controller))
                result.Message = "Nombre del controlador no es valido, ";
            if (string.IsNullOrEmpty(request.Action))
                result.Message += "Nombre de la accion no es valido, ";
            if (string.IsNullOrEmpty(request.Name))
            {
                result.Success = false;
                result.Message += "Nombre del SubMenu no es valido";
                return result;
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var subMenu = db.SubMenu.Where(s => s.Name == request.Name).FirstOrDefault();
                if(subMenu == null)
                {
                    SubMenu sub = new SubMenu() {
                        IdMenu = request.IdMenu,
                        Name = request.Name,
                        ControllerName = request.Controller,
                        ActionName = request.Action,
                        IsActive = request.IsActive,
                        CreationDate = DateTime.Now,
                    };
                    db.SubMenu.Add(sub);
                    db.SaveChanges();
                }
                else
                {
                    subMenu.Name = request.Name;
                    subMenu.ControllerName = request.Controller;
                    subMenu.ActionName = request.Action;
                    subMenu.IsActive = request.IsActive;
                    db.Entry(subMenu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();                    
                }                
            }
            return result;
        }

        public ResultJson CreateRol(RequestRol request)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<ApplicationRole>(context);
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.InitialPageUrl))
                {
                    result.Success = false;
                    result.Message = "Nombre, Page URL es obligatorio";
                    return result;
                }

                var role = roleManager.FindByName(request.Name);
                if (role != null)
                {
                    result.Success = false;
                    result.Message = "El nombre del rol ya existe";
                    return result;                   
                }
                

                role = new ApplicationRole() { Name = request.Name, Description = request.Description, InitialPageUrl = request.InitialPageUrl };
                var rolResult = roleManager.Create(role);  
                return result;
            }
        }

        public ResultJson CreateUser(RequesUser request)
        {
            if (string.IsNullOrEmpty(request.Nombre))
            {
                result.Success = false;
                result.Message = "Transacción cancelada";
                return result;
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                const string password = "loyalty";

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = new ApplicationUser() { UserName = request.Email, Email = request.Email, Profiles = new Profiles() { FirstName = request.Nombre, MiddleName = request.Ape_paterno, LastName = request.Ape_materno, Age = request.Age } };
                var userResult = UserManager.Create(user, password);
                userResult = UserManager.SetLockoutEnabled(user.Id, false);

                var rolesForUser = UserManager.GetRoles(user.Id);
                if (!rolesForUser.Contains("Participante"))
                {
                    var result = UserManager.AddToRole(user.Id, "Participante");
                }

            }
            return result;
        }

        public ResultJson DeleteAction(int Id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var action = db.SubMenu.Find(Id);
                if(action == null)
                {
                    result.Success = false;
                    result.Message = "El código de acción no existe";
                    return result;
                }
                db.Entry(action).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            return result;
        }

        public ResultJson UpdateRol(RequestRol request)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<ApplicationRole>(context);
                var roleMngr = new RoleManager<ApplicationRole>(roleStore);

                var roles = roleMngr.Roles.Where(s => s.Id == request.Id).FirstOrDefault();
                if (roles == null)
                {
                    result.Success = false;
                    result.Message = "No existe el rol indicado";
                    return result;
                }

                roles.Description = request.Description;
                roles.InitialPageUrl = request.InitialPageUrl;
                context.Entry(roles).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return result;
            }
        }

        public ResultJson UpdateUser(RequesUser request)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Profiles.Find(request.Key);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado";
                    return result;
                }
                user.FirstName = request.Nombre;
                user.MiddleName = request.Ape_paterno;
                user.LastName = request.Ape_materno;
                user.Age = request.Age;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return result;
        }

        public ResultJson UpdateUserAccess(string key, string arrayKey)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (key == null || string.IsNullOrEmpty(arrayKey) || string.IsNullOrEmpty(key))
                {
                    result.Success = false;
                    result.Message = "Transacción cancelada, elige una o varias opciones";
                    return result;
                }

                #region delete filters  
                var currentPer = context.AccessByUser.Where(s => s.IdUser == key).ToList();
                foreach (var per in currentPer)
                {
                    context.Entry(per).State = System.Data.Entity.EntityState.Deleted;
                    context.SaveChanges();
                }
                #endregion

                #region add new filters
                var keyList = arrayKey.Split(',');
                List<AccessByUser> addPer = new List<AccessByUser>();
                foreach (var item in keyList)
                {
                    addPer.Add(new AccessByUser() { IdUser = key, IdSubMenu = int.Parse(item) });
                }
                context.AccessByUser.AddRange(addPer);
                context.SaveChanges();
                #endregion
            }
            return result;
        }


        public ResultJson ALDistribuidor(string key, string arrayKey)
        {
            using (DbContextJulio context = new DbContextJulio())
            {
                if (string.IsNullOrEmpty(arrayKey) || string.IsNullOrEmpty(key))
                {
                    result.Success = false;
                    result.Message = "Elige una o varias opciones de distribuidor";
                    return result;
                }

                #region Eliminar relación de usuario distribuidor
                var ListaDistribuidores = context.AspNetUsers_Distribuidor.Where(s => s.IdUser == key).ToList();
                foreach (var distribuidor in ListaDistribuidores)
                {
                    context.Entry(distribuidor).State = System.Data.Entity.EntityState.Deleted;
                    context.SaveChanges();
                }
                #endregion

                #region Agregar realación de usuario distribuidor
                var keyList = arrayKey.Split(',');
                List<Model.EntitiesModels.AspNetUsers_Distribuidor> addDistribuidor = new List<Model.EntitiesModels.AspNetUsers_Distribuidor>();
                foreach (var item in keyList)
                {
                    addDistribuidor.Add(new Model.EntitiesModels.AspNetUsers_Distribuidor() { IdUser = key, IdDistribuidor = int.Parse(item) });
                }
                context.AspNetUsers_Distribuidor.AddRange(addDistribuidor);
                context.SaveChanges();
                #endregion
            }
            return result;
        }


    }
}
