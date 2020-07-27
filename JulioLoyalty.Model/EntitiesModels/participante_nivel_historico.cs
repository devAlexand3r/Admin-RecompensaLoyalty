namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class participante_nivel_historico
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? participante_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? nivel_id { get; set; }

        public DateTime? fecha_nivel { get; set; }

        public DateTime? fecha_captura { get; set; }
    }
}
