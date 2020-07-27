namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class status_premio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public status_premio()
        {
            historial_status_premio = new HashSet<historial_status_premio>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(10)]
        public string clave { get; set; }

        [Required]
        [StringLength(30)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string descripcion_larga { get; set; }

        [Column(TypeName = "numeric")]
        public decimal usuario_alta_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? usuario_cambio_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? usuario_baja_id { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        [StringLength(10)]
        public string clave_rms { get; set; }

        [StringLength(100)]
        public string descripcion_rms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historial_status_premio> historial_status_premio { get; set; }
    }
}
