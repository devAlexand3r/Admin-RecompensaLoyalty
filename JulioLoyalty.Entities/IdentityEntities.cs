using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Entities.Menu;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JulioLoyalty.Entities
{
    // Puede agregar datos del perfil del usuario agregando más propiedades a la clase ApplicationUser.
    public class ApplicationUser : IdentityUser
    {
        public virtual Profiles Profiles { get; set; }
        public virtual List<AccessByUser> AccessByUser { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }
    
    /// <summary>
    /// Clase POCO para agregar a un rol su decripción
    /// </summary>
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Description { get; set; }
        public string InitialPageUrl { get; set; }
    }
}
