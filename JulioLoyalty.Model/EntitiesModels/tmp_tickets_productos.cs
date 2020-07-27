namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_tickets_productos
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ticket_id { get; set; }

        [StringLength(255)]
        public string ClvProducto { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        public int? Cantidad { get; set; }

        public double? MontoSD { get; set; }

        public double? MontoCD { get; set; }

        public virtual tmp_tickets tmp_tickets { get; set; }
    }
}
