namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class participante_tarjeta
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? participante_id { get; set; }

        [StringLength(100)]
        public string tarjeta { get; set; }

        [StringLength(20)]
        public string codigo { get; set; }

        [StringLength(10)]
        public string addon { get; set; }

        [StringLength(30)]
        public string codbar { get; set; }

        [StringLength(1)]
        public string digito_control { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? status_tarjeta_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tipo_tarjeta_id { get; set; }

        public DateTime? fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid? usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pais_id { get; set; }

        public string justificacion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? distribuidor_id { get; set; }

        public virtual distribuidor distribuidor { get; set; }
    }
}
