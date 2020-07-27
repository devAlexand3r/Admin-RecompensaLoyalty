namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class proveedor_premios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public proveedor_premios()
        {
            premio = new HashSet<premio>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(30)]
        public string clave { get; set; }

        [Required]
        [StringLength(130)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(1000)]
        public string descripcion_larga { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pais_id { get; set; }

        [StringLength(255)]
        public string referencia_campania { get; set; }

        [StringLength(255)]
        public string datos_facturacion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal usuario_alta_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? usuario_cambio_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? usuario_baja_id { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public virtual pais pais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<premio> premio { get; set; }
    }
}
