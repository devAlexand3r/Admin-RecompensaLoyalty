namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class saldos_participante
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal categoria_transaccion_id { get; set; }

        public int? puntos { get; set; }

        public DateTime? fecha { get; set; }

        public virtual participante participante { get; set; }
    }
}
