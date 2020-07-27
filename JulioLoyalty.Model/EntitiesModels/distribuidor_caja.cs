namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class distribuidor_caja
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal distribuidor_id { get; set; }

        [StringLength(20)]
        public string clave { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [StringLength(250)]
        public string descripcion_larga { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        public virtual distribuidor distribuidor { get; set; }
    }
}
