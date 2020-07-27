namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("producto")]
    public partial class producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public producto()
        {
            transaccion_detalle = new HashSet<transaccion_detalle>();
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

        [Required]
        [StringLength(255)]
        public string clave_ean { get; set; }

        [Required]
        [StringLength(255)]
        public string clave_dun { get; set; }

        [Column(TypeName = "numeric")]
        public decimal temporada_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? rama_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal categoria_producto_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? linea_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? familia_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? color_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? talla_id { get; set; }

        public double? precio_lista { get; set; }

        public double? precio_publico { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        public virtual categoria_producto categoria_producto { get; set; }

        public virtual producto producto1 { get; set; }

        public virtual producto producto2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaccion_detalle> transaccion_detalle { get; set; }
    }
}
