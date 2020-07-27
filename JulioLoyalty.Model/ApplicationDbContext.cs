using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using JulioLoyalty.Entities;
using JulioLoyalty.Model.Mappings;
using JulioLoyalty.Model.Mappings.Menu;

namespace JulioLoyalty.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("ConnectionLoyalty", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<Entities.Menu.Menu> Menu { get ; set; }
        public DbSet<Entities.Menu.SubMenu> SubMenu { get; set; }
        public DbSet<Entities.Menu.AccessByUser> AccessByUser { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProfileMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new SubMenuMap());
            modelBuilder.Configurations.Add(new AccessByUserMap());
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}
