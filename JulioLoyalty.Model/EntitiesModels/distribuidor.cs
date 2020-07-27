namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("distribuidor")]
    public partial class distribuidor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public distribuidor()
        {
            almacen_tarjetas = new HashSet<almacen_tarjetas>();
            AspNetUsers_Distribuidor = new HashSet<AspNetUsers_Distribuidor>();
            distribuidor_caja = new HashSet<distribuidor_caja>();
            distribuidor_direccion = new HashSet<distribuidor_direccion>();
            participante = new HashSet<participante>();
            participante_tarjeta = new HashSet<participante_tarjeta>();
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

        [StringLength(250)]
        public string url_imagen { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal pais_id { get; set; }

        [StringLength(20)]
        public string prefijo_rms { get; set; }

        [Column(TypeName = "text")]
        public string texto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<almacen_tarjetas> almacen_tarjetas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUsers_Distribuidor> AspNetUsers_Distribuidor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<distribuidor_caja> distribuidor_caja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<distribuidor_direccion> distribuidor_direccion { get; set; }

        public virtual pais pais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participante> participante { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participante_tarjeta> participante_tarjeta { get; set; }
    }
}
