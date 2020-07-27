namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CatalogoErroresWS
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal ErrorId { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Detalle { get; set; }
    }
}
