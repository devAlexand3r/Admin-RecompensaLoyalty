namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class agenda_llamadas
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal tipo_llamada_id { get; set; }

        [StringLength(50)]
        public string titulo { get; set; }

        [StringLength(500)]
        public string descripcion { get; set; }

        public DateTime fecha { get; set; }

        [Column(TypeName = "numeric")]
        public decimal status_llamada_id { get; set; }

        public DateTime? fecha_status { get; set; }

        public virtual participante participante { get; set; }

        public virtual status_llamada status_llamada { get; set; }

        public virtual tipo_llamada tipo_llamada { get; set; }
    }
}
