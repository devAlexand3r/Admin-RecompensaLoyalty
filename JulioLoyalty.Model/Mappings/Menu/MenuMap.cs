using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace JulioLoyalty.Model.Mappings.Menu
{
    public class MenuMap : EntityTypeConfiguration<Entities.Menu.Menu>
    {
        public MenuMap()
        {
            HasKey(e => e.Id).ToTable("Menu");
            Property(e => e.Name).HasMaxLength(150).IsRequired();            
            Property(e => e.Description).HasMaxLength(250).IsOptional();
            Property(e => e.Index).IsOptional();
            Property(e => e.CreationDate).IsRequired();
        }
    }
}
