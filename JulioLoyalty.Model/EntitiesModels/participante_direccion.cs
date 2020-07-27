namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class participante_direccion
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal tipo_direccion_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }
        
        [StringLength(100)]
        public string calle { get; set; }

        [StringLength(20)]
        public string numero_interior { get; set; }

        [StringLength(20)]
        public string numero_exterior { get; set; }

        [StringLength(50)]
        public string entrecalle_1 { get; set; }

        [StringLength(50)]
        public string entrecalle_2 { get; set; }

        [StringLength(100)]
        public string colonia { get; set; }

        [StringLength(8)]
        public string codigo_postal { get; set; }

        [StringLength(500)]
        public string referencias { get; set; }

        [Column(TypeName = "numeric")]
        public decimal status_id { get; set; }

        public DateTime fecha_status { get; set; }

        public Guid usuario_id { get; set; }

        [StringLength(256)]
        public string estado { get; set; }

        [StringLength(256)]
        public string municipio { get; set; }

        public virtual participante participante { get; set; }

        public virtual status status { get; set; }

        public virtual tipo_direccion tipo_direccion { get; set; }
    }
}
