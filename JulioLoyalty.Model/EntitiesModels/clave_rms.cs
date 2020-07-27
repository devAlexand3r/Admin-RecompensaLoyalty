namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class clave_rms
    {
        public int id { get; set; }

        [Required]
        [StringLength(4)]
        public string anio { get; set; }

        [Required]
        [StringLength(7)]
        public string clave { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pais_id { get; set; }

        public virtual pais pais { get; set; }
    }
}
