namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("participante")]
    public partial class participante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public participante()
        {
            agenda_llamadas = new HashSet<agenda_llamadas>();
            AspNetUsers_Participante = new HashSet<AspNetUsers_Participante>();
            error_recarga_sin_saldo_suficiente = new HashSet<error_recarga_sin_saldo_suficiente>();
            historico_tarjetas = new HashSet<historico_tarjetas>();
            llamada = new HashSet<llamada>();
            no_participan = new HashSet<no_participan>();
            participante_direccion = new HashSet<participante_direccion>();
            participante_telefono = new HashSet<participante_telefono>();
            participante_status_comentarios = new HashSet<participante_status_comentarios>();
            saldos_participante = new HashSet<saldos_participante>();
            transaccion = new HashSet<transaccion>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(30)]
        public string clave { get; set; }

        [StringLength(100)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string apellido_paterno { get; set; }

        [StringLength(50)]
        public string apellido_materno { get; set; }

        [StringLength(30)]
        public string documento_identidad { get; set; }

        [StringLength(80)]
        public string correo_electronico { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? sexo_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? estado_civil_id { get; set; }

        public DateTime? fecha_nacimiento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? distribuidor_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tipo_participante_id { get; set; }

        public Guid? usuario_alta { get; set; }

        public DateTime? fecha_alta { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? status_participante_id { get; set; }

        public DateTime? fecha_status { get; set; }

        public Guid? usuario_status { get; set; }

        public int? momento_favorito { get; set; }

        public int? frecuencia_compra { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ocupacion_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<agenda_llamadas> agenda_llamadas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUsers_Participante> AspNetUsers_Participante { get; set; }

        public virtual distribuidor distribuidor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<error_recarga_sin_saldo_suficiente> error_recarga_sin_saldo_suficiente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historico_tarjetas> historico_tarjetas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<llamada> llamada { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<no_participan> no_participan { get; set; }

        public virtual ocupacion ocupacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participante_direccion> participante_direccion { get; set; }

        public virtual sexo sexo { get; set; }

        public virtual status_participante status_participante { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participante_telefono> participante_telefono { get; set; }

        public virtual tipo_participante tipo_participante { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participante_status_comentarios> participante_status_comentarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saldos_participante> saldos_participante { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transaccion> transaccion { get; set; }
    }
}
