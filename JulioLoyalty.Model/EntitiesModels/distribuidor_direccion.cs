namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class distribuidor_direccion
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal distribuidor_id { get; set; }

        [Required]
        [StringLength(150)]
        public string contacto { get; set; }

        [StringLength(50)]
        public string razon_social { get; set; }

        [StringLength(150)]
        public string telefono { get; set; }

        [StringLength(150)]
        public string calle { get; set; }

        [StringLength(50)]
        public string entre_calle { get; set; }

        [StringLength(50)]
        public string y_calle { get; set; }

        [StringLength(50)]
        public string colonia { get; set; }

        [StringLength(50)]
        public string numero { get; set; }

        [StringLength(50)]
        public string municipio { get; set; }

        [StringLength(50)]
        public string estado { get; set; }

        [StringLength(50)]
        public string codigo_postal { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_cambio { get; set; }

        public DateTime? fecha_baja { get; set; }

        public Guid usuario_alta_id { get; set; }

        public Guid? usuario_cambio_id { get; set; }

        public Guid? usuario_baja_id { get; set; }

        public virtual distribuidor distribuidor { get; set; }
    }
}
