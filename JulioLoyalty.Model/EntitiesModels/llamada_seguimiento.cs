namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class llamada_seguimiento
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal llamada_id { get; set; }

        [Required]
        [StringLength(100)]
        public string observacion { get; set; }

        public DateTime fecha { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tipo_llamada_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal status_seguimiento_id { get; set; }

        public Guid usuario_id { get; set; }

        public virtual llamada llamada { get; set; }

        public virtual status_seguimiento status_seguimiento { get; set; }

        public virtual tipo_llamada tipo_llamada { get; set; }
    }
}
