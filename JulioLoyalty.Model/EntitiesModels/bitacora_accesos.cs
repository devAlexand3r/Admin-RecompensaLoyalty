namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bitacora_accesos
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        public Guid usuario_id { get; set; }

        public DateTime fecha { get; set; }

        [StringLength(150)]
        public string opcion { get; set; }

        [StringLength(150)]
        public string evento { get; set; }
    }
}
