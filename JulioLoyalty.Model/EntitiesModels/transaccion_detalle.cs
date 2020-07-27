namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class transaccion_detalle
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal transaccion_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? producto_id { get; set; }

        public double? cantidad { get; set; }

        public double? importe { get; set; }

        public int? puntos { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? historico_id { get; set; }

        public virtual producto producto { get; set; }

        public virtual transaccion transaccion { get; set; }
    }
}
