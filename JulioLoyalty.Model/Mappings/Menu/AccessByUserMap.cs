using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace JulioLoyalty.Model.Mappings.Menu
{
    public class AccessByUserMap : EntityTypeConfiguration<Entities.Menu.AccessByUser>
    {
        public AccessByUserMap()
        {
            HasKey(e => e.Id).ToTable("AccessByUser");
            Property(e => e.IdUser).HasMaxLength(128);
            HasRequired(e => e.ApplicationUser).WithMany(e => e.AccessByUser).HasForeignKey(e => e.IdUser).WillCascadeOnDelete(false);
            HasRequired(e => e.SubMenu).WithMany(e => e.AccessByUser).HasForeignKey(s => s.IdSubMenu).WillCascadeOnDelete(false);  
        }
    }
}
