namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class transaccion_premio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public transaccion_premio()
        {
            error_recarga_sin_saldo_suficiente = new HashSet<error_recarga_sin_saldo_suficiente>();
            historial_status_premio = new HashSet<historial_status_premio>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal transaccion_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal premio_id { get; set; }

        public int puntos { get; set; }

        public int? pedido_rms { get; set; }

        public int? direccion_entrega { get; set; }

        [StringLength(100)]
        public string observaciones { get; set; }

        [StringLength(50)]
        public string folio_confirmacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<error_recarga_sin_saldo_suficiente> error_recarga_sin_saldo_suficiente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historial_status_premio> historial_status_premio { get; set; }

        public virtual premio premio { get; set; }

        public virtual transaccion transaccion { get; set; }
    }
}
