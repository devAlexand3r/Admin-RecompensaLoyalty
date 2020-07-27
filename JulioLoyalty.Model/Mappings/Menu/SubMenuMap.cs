using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace JulioLoyalty.Model.Mappings.Menu
{
    public class SubMenuMap : EntityTypeConfiguration<Entities.Menu.SubMenu>
    {
        public SubMenuMap()
        {
            HasKey(e => e.Id).ToTable("SubMenu");
            HasRequired(e => e.Menu).WithMany(e => e.SubMenu).HasForeignKey(s => s.IdMenu).WillCascadeOnDelete(false);
            Property(e => e.Name).HasMaxLength(150).IsRequired();
            Property(e => e.ControllerName).HasMaxLength(50).IsRequired();
            Property(e => e.ActionName).HasMaxLength(50).IsRequired();
            Property(e => e.CreationDate).IsRequired();
            Property(e => e.IsActive).IsRequired();
        }
    }
}
