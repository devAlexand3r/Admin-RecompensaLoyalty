namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("premio")]
    public partial class premio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public premio()
        {
            error_recarga_sin_saldo_suficiente = new HashSet<error_recarga_sin_saldo_suficiente>();
            transaccion_premio = new HashSet<transaccion_premio>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(20)]
        public string clave { get; set; }

        [StringLength(350)]
        public string descripcion { get; set; }

        [StringLength(700)]
        public string descripcion_larga { get; set; }

        [Required]
        [StringLength(255)]
        public string url_imagen_small { get; set; }

        [StringLength(255)]
        public string url_imagen_large { get; set; }

        [Column(TypeName = "numeric")]
        public decimal categoria_premio_id { get; set; }

        public int? puntos { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? proveedor_premios_id { get; set; }

        public double? costo { get; set; }

        public double? precio { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        public double? iva { get; set; }

        [Column(TypeName = "numeric")]
        public decimal marca_premio_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? operadora_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tipo_envio_id { get; set; }

        public virtual categoria_premio categoria_premio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<error_recarga_sin_saldo_suficiente> error_recarga_sin_saldo_suficiente { get; set; }

        public virtual marca_premio marca_premio { get; set; }

        public virtual operadora operadora { get; set; }

        public virtual proveedor_premios proveedor_premios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaccion_premio> transaccion_premio { get; set; }
    }
}
