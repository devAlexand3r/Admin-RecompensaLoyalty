using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business
{
    public static class Bicatora
    {
        public static void RegistrarAcceso(string username, string _opcion, string _evento)
        {
            // Registrar acceso por usuario
            using (Model.DbContextJulio db = new Model.DbContextJulio())
            {
                var user = db.AspNetUsers.Where(s => s.UserName == username).FirstOrDefault();
                if (user != null)
                {
                    Model.EntitiesModels.bitacora_accesos bitacora = new Model.EntitiesModels.bitacora_accesos()
                    {
                        usuario_id = Guid.Parse(user.Id),
                        fecha = DateTime.Now,
                        opcion = _opcion,
                        evento = _evento
                    };
                    db.bitacora_accesos.Add(bitacora);
                    db.SaveChanges();
                }
            }
        }
    }
}
