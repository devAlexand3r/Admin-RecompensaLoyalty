namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bitacora_activaciones
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [StringLength(20)]
        public string actividad { get; set; }

        [StringLength(20)]
        public string tarjeta { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? distribuidor_id { get; set; }

        [StringLength(20)]
        public string numero_ticket { get; set; }

        public Guid? usuario_id { get; set; }

        public DateTime? fecha { get; set; }
    }
}
