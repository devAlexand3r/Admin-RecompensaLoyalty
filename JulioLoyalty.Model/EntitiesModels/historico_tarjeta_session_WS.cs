namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_tarjeta_session_WS
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [StringLength(100)]
        public string tarjeta { get; set; }

        public Guid? sessionID { get; set; }

        public DateTime? fecha { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? participante_id { get; set; }
    }
}
