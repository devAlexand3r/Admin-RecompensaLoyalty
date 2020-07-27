namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_agentes
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [StringLength(255)]
        public string Clave { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Tipo { get; set; }

        [StringLength(255)]
        public string Estatus { get; set; }
    }
}
