namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_ventas_producto
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(13)]
        public string clave_ean { get; set; }

        [StringLength(50)]
        public string Producto { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Cantidad { get; set; }

        public double ImporteProducto { get; set; }

        public double? ImporteProductoPlista { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? vendedor_id { get; set; }

        public DateTime fecha { get; set; }

        [Column(TypeName = "numeric")]
        public decimal historico_ventas_id { get; set; }

        [StringLength(1)]
        public string estatusVenta { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cantidad_Devuelta { get; set; }

        public DateTime? fecha_devolucion { get; set; }

        public virtual historico_ventas historico_ventas { get; set; }
    }
}
