namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_ventas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public historico_ventas()
        {
            historico_acumulacion = new HashSet<historico_acumulacion>();
            historico_ventas_forma_pago = new HashSet<historico_ventas_forma_pago>();
            historico_ventas_producto = new HashSet<historico_ventas_producto>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(50)]
        public string tarjeta { get; set; }

        [Required]
        [StringLength(50)]
        public string tiendaid { get; set; }

        [Required]
        [StringLength(50)]
        public string Posid { get; set; }

        [StringLength(50)]
        public string transaccionid { get; set; }

        [Required]
        [StringLength(50)]
        public string Numeroticket { get; set; }

        public double Importeticket { get; set; }

        [Required]
        [StringLength(50)]
        public string Key { get; set; }

        public DateTime fechaventa { get; set; }

        [StringLength(1)]
        public string estatusVenta { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        [StringLength(50)]
        public string CodigoPromo { get; set; }

        [StringLength(1)]
        public string aplicabono { get; set; }

        [StringLength(10)]
        public string numero_autorizacion { get; set; }

        [StringLength(50)]
        public string folio_sbx { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historico_acumulacion> historico_acumulacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historico_ventas_forma_pago> historico_ventas_forma_pago { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historico_ventas_producto> historico_ventas_producto { get; set; }
    }
}
