namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class pais
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pais()
        {
            agente = new HashSet<agente>();
            caja = new HashSet<caja>();
            categoria_premio = new HashSet<categoria_premio>();
            categoria_producto = new HashSet<categoria_producto>();
            categoria_tipo_llamada = new HashSet<categoria_tipo_llamada>();
            categoria_transaccion = new HashSet<categoria_transaccion>();
            clave_rms = new HashSet<clave_rms>();
            color = new HashSet<color>();
            distribuidor = new HashSet<distribuidor>();
            estado_civil = new HashSet<estado_civil>();
            familia = new HashSet<familia>();
            forma_pago = new HashSet<forma_pago>();
            linea = new HashSet<linea>();
            ocupacion = new HashSet<ocupacion>();
            operadora = new HashSet<operadora>();
            proveedor_premios = new HashSet<proveedor_premios>();
            rama = new HashSet<rama>();
            sexo = new HashSet<sexo>();
            tema = new HashSet<tema>();
            status_carga = new HashSet<status_carga>();
            status_llamada = new HashSet<status_llamada>();
            status_tarjeta = new HashSet<status_tarjeta>();
            status_transaccion = new HashSet<status_transaccion>();
            talla = new HashSet<talla>();
            temporada = new HashSet<temporada>();
            tipo_agente = new HashSet<tipo_agente>();
            tipo_pago = new HashSet<tipo_pago>();
            tipo_tarjeta = new HashSet<tipo_tarjeta>();
            vendedor = new HashSet<vendedor>();
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

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        [StringLength(100)]
        public string correo_electronico { get; set; }

        [StringLength(50)]
        public string remitente { get; set; }

        [StringLength(100)]
        public string servidor_pop { get; set; }

        [StringLength(100)]
        public string servidor_smtp { get; set; }

        [StringLength(50)]
        public string usuario_correo { get; set; }

        [StringLength(20)]
        public string password_correo { get; set; }

        [StringLength(50)]
        public string prefijo_rms { get; set; }

        [StringLength(20)]
        public string usuario_rms { get; set; }

        [StringLength(20)]
        public string password_rms { get; set; }

        public double? valor_punto { get; set; }

        [StringLength(250)]
        public string url_programa { get; set; }

        [StringLength(5)]
        public string clave_carga { get; set; }

        [StringLength(250)]
        public string url_logo { get; set; }

        public string plantilla_aviso { get; set; }

        public string plantilla_terminos { get; set; }

        public string plantilla_faqs { get; set; }

        public string plantilla_entrega_premios { get; set; }

        public byte? tipo_envio_digital { get; set; }

        public byte? tipo_envio_recarga { get; set; }

        public byte? tipo_envio_fisico { get; set; }

        public string banner_carousel { get; set; }

        [StringLength(255)]
        public string theme { get; set; }

        public string Aviso_de_privacidad { get; set; }
        public string Terminos_y_condiciones { get; set; }
        public string Que_Es_Recompensas_Loyalty { get; set; }
        public string Reglamento_del_programa { get; set; }
        public string Como_canjeo_mis_puntos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<agente> agente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<caja> caja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<categoria_premio> categoria_premio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<categoria_producto> categoria_producto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<categoria_tipo_llamada> categoria_tipo_llamada { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<categoria_transaccion> categoria_transaccion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<clave_rms> clave_rms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<color> color { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<distribuidor> distribuidor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<estado_civil> estado_civil { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<familia> familia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<forma_pago> forma_pago { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<linea> linea { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ocupacion> ocupacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<operadora> operadora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proveedor_premios> proveedor_premios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rama> rama { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sexo> sexo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tema> tema { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<status_carga> status_carga { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<status_llamada> status_llamada { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<status_tarjeta> status_tarjeta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<status_transaccion> status_transaccion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<talla> talla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<temporada> temporada { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tipo_agente> tipo_agente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tipo_pago> tipo_pago { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tipo_tarjeta> tipo_tarjeta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vendedor> vendedor { get; set; }
    }
}
