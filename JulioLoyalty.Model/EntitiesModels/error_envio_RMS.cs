namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class error_envio_RMS
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        public DateTime fecha { get; set; }

        [Column(TypeName = "numeric")]
        public decimal transaccion_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pedido_rms { get; set; }

        [Column(TypeName = "numeric")]
        public decimal tipo_direccion_id { get; set; }

        [Required]
        [StringLength(5)]
        public string country_code { get; set; }

        public string mensaje { get; set; }

        public bool status { get; set; }
    }
}
