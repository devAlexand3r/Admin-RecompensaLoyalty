namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("llamada")]
    public partial class llamada
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public llamada()
        {
            llamada_seguimiento = new HashSet<llamada_seguimiento>();
            llamada_tipo_llamada = new HashSet<llamada_tipo_llamada>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_llama { get; set; }

        [StringLength(30)]
        public string telefono { get; set; }

        public DateTime fecha { get; set; }

        public string descripcion { get; set; }

        public Guid usuario_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? agenda_llamadas_id { get; set; }

        [StringLength(100)]
        public string correo_electronico { get; set; }

        public virtual participante participante { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<llamada_seguimiento> llamada_seguimiento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<llamada_tipo_llamada> llamada_tipo_llamada { get; set; }
    }
}
