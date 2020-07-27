namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_catproductos
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [StringLength(255)]
        public string NumArticulo { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        [StringLength(255)]
        public string Temporada { get; set; }

        [StringLength(255)]
        public string Rama { get; set; }

        [StringLength(255)]
        public string Categoria { get; set; }

        [StringLength(255)]
        public string Linea { get; set; }

        [StringLength(255)]
        public string Familia { get; set; }

        [StringLength(255)]
        public string Codigo { get; set; }

        public double? PrecioLista { get; set; }

        public double? Precio2 { get; set; }

        [StringLength(255)]
        public string Color { get; set; }

        [StringLength(255)]
        public string Talla { get; set; }
    }
}
