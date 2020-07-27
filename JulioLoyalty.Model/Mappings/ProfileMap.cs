using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Entities;
using System.Data.Entity.ModelConfiguration;

namespace JulioLoyalty.Model.Mappings
{
    public class ProfileMap : EntityTypeConfiguration<Profiles>
    {
        public ProfileMap()
        {
            HasKey(p => p.Id).ToTable("AspNetProfiles");
            HasRequired(p => p.ApplicationUser).WithOptional(p => p.Profiles).WillCascadeOnDelete();
            Property(p => p.Id).HasMaxLength(128);
            Property(p => p.FirstName).HasMaxLength(250).IsOptional();
            Property(p => p.MiddleName).HasMaxLength(250).IsOptional();
            Property(p => p.LastName).HasMaxLength(250).IsOptional();
        }
    }
}
