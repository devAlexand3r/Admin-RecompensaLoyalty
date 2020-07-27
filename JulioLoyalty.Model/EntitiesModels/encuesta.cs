namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("encuesta")]
    public partial class encuesta
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal tipo_encuesta_id { get; set; }

        [Required]
        [StringLength(500)]
        public string nombre { get; set; }

        public DateTime fecha_inicial { get; set; }

        public DateTime fecha_final { get; set; }

        [Column(TypeName = "numeric")]
        public decimal status_id { get; set; }

        public DateTime fecha_status { get; set; }

        [StringLength(250)]
        public string guion { get; set; }

        [StringLength(500)]
        public string cierre { get; set; }

        [Column(TypeName = "image")]
        public byte[] logo { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }
    }
}
