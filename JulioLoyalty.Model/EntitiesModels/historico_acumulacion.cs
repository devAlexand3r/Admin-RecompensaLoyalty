namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_acumulacion
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal historico_ventas_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal producto_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal cantidad { get; set; }

        public double ImporteProducto { get; set; }

        [Column(TypeName = "numeric")]
        public decimal tipo_pago_id { get; set; }

        public double? importe_tipo_pago { get; set; }

        public byte? acumula { get; set; }

        public byte? sin_descuento { get; set; }

        public int? puntos { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal nivel_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? historico_ventas_producto_id { get; set; }

        public virtual historico_ventas historico_ventas { get; set; }
    }
}
