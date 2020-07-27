namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class participante_telefono
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal tipo_telefono_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Required]
        [StringLength(5)]
        public string lada { get; set; }

        [Required]
        [StringLength(20)]
        public string telefono { get; set; }

        [StringLength(20)]
        public string extension { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? operadora_id { get; set; }

        public virtual operadora operadora { get; set; }

        public virtual participante participante { get; set; }

        public virtual tipo_telefono tipo_telefono { get; set; }
    }
}
