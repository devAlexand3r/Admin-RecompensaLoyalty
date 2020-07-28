
using JulioLoyalty.Entities;
using JulioLoyalty.Model;
using JulioLoyalty.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace JulioLoyalty.UI.Controllers.WebAPI
{
    [Authorize]
    public class ServiceController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetMenu()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var menus = context.Menu.Select(s => new
                {
                    s.Id,
                    s.Name,
                    SubMenu = s.SubMenu.Where(x => x.IdMenu == s.Id && x.IsActive == true).Select(y => new
                    {
                        y.Id,
                        y.IdMenu,
                        y.Name,
                        y.IsActive,
                    }).ToList()
                }).ToList();
                return Json(new { data = menus });
            }
        }

        [HttpGet]
        public IHttpActionResult GetSubMenu(int Id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var sub = context.SubMenu.Where(s => s.IdMenu == Id).Select(s => new
                {
                    s.Id,
                    s.IdMenu,
                    s.ControllerName,
                    s.ActionName,
                    s.Name,
                    s.IsActive
                }).ToList();
                return Json(new { data = sub });
            }
        }

        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<ApplicationRole>(context);
                var roleMngr = new RoleManager<ApplicationRole>(roleStore);
                var roles = roleMngr.Roles.Select(s => new { s.Id, s.Name, s.Description, s.InitialPageUrl }).ToList();

                return Json(new { data = roles });
            }
        }

        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<ApplicationRole>(context);
                var roleMngr = new RoleManager<ApplicationRole>(roleStore);

                var users = context.Users.Where(s => s.Roles.FirstOrDefault().RoleId != "f990af52-97e3-4a19-b095-233a383a43a4").Select(s => new
                {
                    s.Id,
                    s.Email,
                    s.UserName,
                    s.Roles.FirstOrDefault().RoleId,
                    s.Profiles.FirstName,
                    s.Profiles.MiddleName,
                    s.Profiles.LastName,
                    s.Profiles.Age,
                    s.Roles.Count,
                    Roles = roleMngr.Roles.Where(x => x.Id == s.Roles.FirstOrDefault().RoleId).Select(x => new
                    {
                        x.Id,
                        x.Name
                    }).ToList(),
                }).ToList();

                return Json(new { data = users });
            }
        }

        [HttpGet]
        public IHttpActionResult GetSubMenusByUser(string key)
        {
            Business.RenderMenu.IRepositoryMenu repositorymenu = new Business.RenderMenu.MenuDefault(key);
            var sub = repositorymenu.GetSubMenuList().Select(s => new { s.Id, s.IdMenu, s.Name, s.IsActive }).ToList();
            return Json(new { data = sub });
        }

        [AuditFilterApi]
        [HttpGet]
        public IHttpActionResult GetSocias(string tarjeta)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<ApplicationRole>(context);
                var roleMngr = new RoleManager<ApplicationRole>(roleStore);

                var users = context.Users.Where(s => s.Roles.FirstOrDefault().RoleId == "f990af52-97e3-4a19-b095-233a383a43a4" && s.UserName.Contains(tarjeta)).Take(200).Select(s => new
                {
                    s.Id,
                    s.Email,
                    s.UserName,
                    s.Roles.FirstOrDefault().RoleId,
                    s.Profiles.FirstName,
                    s.Profiles.MiddleName,
                    s.Profiles.LastName,
                    s.Profiles.Age,
                    s.Roles.Count,
                    Roles = roleMngr.Roles.Where(x => x.Id == s.Roles.FirstOrDefault().RoleId).Select(x => new
                    {
                        x.Id,
                        x.Name
                    }).ToList(),
                }).ToList();

                return Json(new { data = users });
            }
        }



        //api/Service/ObtenerDistribuidores
        [HttpGet]
        public IHttpActionResult ObtenerDistribuidores()
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var result = db.distribuidor.Select(s => new
                {
                    s.id,
                    s.descripcion,
                    s.descripcion_larga
                }).Distinct().ToList();

                return Json(result);
            };
        }

        [HttpGet]
        public IHttpActionResult DistribuidorPorUsuario(string key)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var result = db.AspNetUsers_Distribuidor.Where(s => s.IdUser == key).Select(s => new
                {
                    s.IdUser,
                    s.IdDistribuidor
                }).ToList();
                return Json(result);
            }
        }

        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        public string consultaTicket(string numTicket)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = db.AspNetUsers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().Id;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@aspnetusers_id", aspnetusers_id);
                parameters.Add("@numTicket", numTicket);
                DataSet setTables = db.GetDataSet("[dbo].[usp_consulta_tickets]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables);
            }
        }


        [AuditFilterApi, APIExceptionFilter]
        [HttpGet]
        public string cancelTicket(string numTicket)
        {
            if (string.IsNullOrEmpty(numTicket))
                return null;

            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = User.Identity.GetUserId();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@ticket", numTicket);
                DataSet setTables = db.GetDataSet("[dbo].[usp_quita_ticket]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> ConsultaProductos(string clave)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var productos = await db.producto.Where(w => w.clave_ean.Contains(clave)).Select(s => new
                {
                    s.id,
                    s.clave,
                    s.descripcion,
                    s.clave_ean,
                    s.precio_lista,
                    s.precio_publico
                }).Take(10).ToListAsync();

                return Json(productos);
            }
        }

        [HttpGet]
        public string GuardaHistoricoVentas(string Numero, string FechaCompra, decimal Total, string Tarjeta)
        {
            DateTime fechaCompra = Convert.ToDateTime(FechaCompra);

            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = User.Identity.GetUserId();
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@tarjeta", Tarjeta  },
                    { "@ticket", Numero },
                    { "@fechaventa", fechaCompra },
                    { "@total", Total }
                };
                DataSet setTables = db.GetDataSet("[dbo].[usp_guarda_historico_ventas]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [HttpGet]
        public string GuardaHistoricoVentasProducto(decimal HistoricoId, string ClaveProducto, decimal Cantidad, decimal Precio)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = User.Identity.GetUserId();
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@historico_ventas_id", HistoricoId  },
                    { "@clave_ean", ClaveProducto },
                    { "@cantidad", Cantidad },
                    { "@precio", Precio }
                };
                DataSet setTables = db.GetDataSet("[dbo].[usp_guarda_historico_ventas_producto]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [HttpGet]
        public string GuardaHistoricoVentasFormaPago(decimal HistoricoId, decimal Puntos)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = User.Identity.GetUserId();
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@historico_ventas_id", HistoricoId  },
                    { "@importe_pago_puntos", Puntos },
                };
                DataSet setTables = db.GetDataSet("[dbo].[usp_guarda_historico_ventas_forma_pago]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [HttpGet]
        public string ObtenerSaldoCliente(decimal Id)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = User.Identity.GetUserId();
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@participante_id", Id  }
                };
                DataSet setTables = db.GetDataSet("[dbo].[sp_obtiene_saldo_participante]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

        [HttpGet]
        public string CalculoPuntosMecanica(decimal HistoricoId)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var aspnetusers_id = User.Identity.GetUserId();
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@histocico_ventas_id", HistoricoId  }
                };
                DataSet setTables = db.GetDataSet("[dbo].[usp_calculo_puntos_mecanica]", CommandType.StoredProcedure, parameters);
                return JsonConvert.SerializeObject(setTables.Tables[0]);
            }
        }

    }
}
