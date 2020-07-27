namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class mailchimp_list
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(50)]
        public string listid { get; set; }

        [Required]
        [StringLength(50)]
        public string clave { get; set; }

        [Required]
        [StringLength(150)]
        public string descripcion { get; set; }
    }
}
