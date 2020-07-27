namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_tarjetas
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Required]
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
        public decimal tipo_tarjeta_id { get; set; }

        public DateTime? fecha_alta { get; set; }

        public DateTime? fecha_inactivacion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal status_tarjeta_id { get; set; }

        [Required]
        public string acciones { get; set; }

        public string justificacion { get; set; }

        public Guid? usuario_alta_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        public virtual participante participante { get; set; }

        public virtual status_tarjeta status_tarjeta { get; set; }

        public virtual tipo_tarjeta tipo_tarjeta { get; set; }
    }
}
