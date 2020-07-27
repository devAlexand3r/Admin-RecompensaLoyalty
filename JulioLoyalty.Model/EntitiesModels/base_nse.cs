namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class base_nse
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [StringLength(10)]
        public string entidad_clave { get; set; }

        [StringLength(50)]
        public string entidad_descripcion { get; set; }

        [StringLength(10)]
        public string municipio_clave { get; set; }

        [StringLength(50)]
        public string municipio_descripcion { get; set; }

        [StringLength(25)]
        public string asentamiento_tipo { get; set; }

        [StringLength(100)]
        public string asentamiento_descripcion { get; set; }

        [StringLength(10)]
        public string nse { get; set; }

        [StringLength(10)]
        public string codigo_postal { get; set; }
    }
}
