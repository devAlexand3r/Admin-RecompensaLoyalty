namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class status_participante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public status_participante()
        {
            participante = new HashSet<participante>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(20)]
        public string clave { get; set; }

        [Required]
        [StringLength(50)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(250)]
        public string descripcion_larga { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? cambia_status_id { get; set; }

        public byte? acumula { get; set; }

        public byte? redime { get; set; }

        [StringLength(3)]
        public string acumula_codigo_mensaje { get; set; }

        [StringLength(250)]
        public string acumula_mensaje { get; set; }

        [StringLength(3)]
        public string redime_codigo_mensaje { get; set; }

        [StringLength(250)]
        public string redime_mensaje { get; set; }

        [StringLength(250)]
        public string acumula_alerta { get; set; }

        [StringLength(250)]
        public string redime_alerta { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal pais_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participante> participante { get; set; }
    }
}
