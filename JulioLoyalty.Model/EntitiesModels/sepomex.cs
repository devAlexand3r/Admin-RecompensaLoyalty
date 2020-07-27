namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sepomex")]
    public partial class sepomex
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [StringLength(50)]
        public string codigo_postal { get; set; }

        [StringLength(250)]
        public string colonia { get; set; }

        [StringLength(50)]
        public string tipo_colonia { get; set; }

        [StringLength(50)]
        public string municipio { get; set; }

        [StringLength(50)]
        public string estado { get; set; }

        [StringLength(50)]
        public string ciudad { get; set; }

        [StringLength(50)]
        public string oficina_postal { get; set; }

        [StringLength(50)]
        public string clave_estado { get; set; }

        [StringLength(50)]
        public string clave_oficina { get; set; }

        [StringLength(50)]
        public string clave_codigo_postal { get; set; }

        [StringLength(50)]
        public string clave_tipo_colonia { get; set; }

        [StringLength(50)]
        public string clave_municipio { get; set; }

        [StringLength(50)]
        public string id_consulta_cp { get; set; }

        [StringLength(50)]
        public string zona { get; set; }

        [StringLength(50)]
        public string clave_ciudad { get; set; }
    }
}
