namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccessByUser")]
    public partial class AccessByUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string IdUser { get; set; }

        public int IdSubMenu { get; set; }
    }
}
