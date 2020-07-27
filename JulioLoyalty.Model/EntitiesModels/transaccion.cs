namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("transaccion")]
    public partial class transaccion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public transaccion()
        {
            error_recarga_sin_saldo_suficiente = new HashSet<error_recarga_sin_saldo_suficiente>();
            transaccion_detalle = new HashSet<transaccion_detalle>();
            transaccion_premio = new HashSet<transaccion_premio>();
            transaccion_comentarios = new HashSet<transaccion_comentarios>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal tipo_transaccion_id { get; set; }

        public DateTime fecha { get; set; }

        public int puntos { get; set; }

        public double importe { get; set; }

        public int? puntos_redimidos { get; set; }

        public Guid usuario_id { get; set; }

        public DateTime fecha_captura { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? estado_cuenta_id { get; set; }

        [StringLength(10)]
        public string numero_autorizacion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? historico_ventas_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<error_recarga_sin_saldo_suficiente> error_recarga_sin_saldo_suficiente { get; set; }

        public virtual participante participante { get; set; }

        public virtual tipo_transaccion tipo_transaccion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaccion_detalle> transaccion_detalle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaccion_premio> transaccion_premio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaccion_comentarios> transaccion_comentarios { get; set; }
    }
}
