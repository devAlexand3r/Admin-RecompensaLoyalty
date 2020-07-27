namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_ventas_forma_pago
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal historico_ventas_id { get; set; }

        [Required]
        [StringLength(50)]
        public string forma_pago { get; set; }

        public double importe_pago { get; set; }

        public virtual historico_ventas historico_ventas { get; set; }
    }
}
