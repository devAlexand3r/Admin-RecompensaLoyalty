namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubMenu")]
    public partial class SubMenu
    {
        public int Id { get; set; }

        public int IdMenu { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string ControllerName { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionName { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsActive { get; set; }

        public virtual Menu Menu { get; set; }
    }
}
