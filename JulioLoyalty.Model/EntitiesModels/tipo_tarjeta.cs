namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tipo_tarjeta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tipo_tarjeta()
        {
            historico_tarjetas = new HashSet<historico_tarjetas>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        public string clave { get; set; }

        [Required]
        public string descripcion { get; set; }

        [Required]
        public string descripcion_larga { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pais_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historico_tarjetas> historico_tarjetas { get; set; }

        public virtual pais pais { get; set; }
    }
}
