namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_tickets_formasdepago
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ticket_id { get; set; }

        [StringLength(255)]
        public string ClvFormaPago { get; set; }

        public double? MontoFP { get; set; }

        [StringLength(255)]
        public string Autorizacion { get; set; }

        public virtual tmp_tickets tmp_tickets { get; set; }
    }
}
