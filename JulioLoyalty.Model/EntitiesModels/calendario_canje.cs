namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class calendario_canje
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        public DateTime fecha_inicial { get; set; }

        public DateTime fecha_final { get; set; }

        [Required]
        public string descripcion { get; set; }

        public string descripcion_larga { get; set; }
    }
}
