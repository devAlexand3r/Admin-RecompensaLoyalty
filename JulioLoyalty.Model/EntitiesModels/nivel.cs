namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nivel")]
    public partial class nivel
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [StringLength(250)]
        public string descripcion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? distribuidor_id { get; set; }

        public double? porcentaje_acumulacion { get; set; }

        public double? valor_nivel_inicial { get; set; }

        public double? valor_nivel_final { get; set; }
    }
}
