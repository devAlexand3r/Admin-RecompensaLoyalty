namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_sucursales
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public double? Sucursal { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Prefijo { get; set; }

        [StringLength(255)]
        public string Direccion { get; set; }

        [StringLength(255)]
        public string Colonia { get; set; }

        [StringLength(255)]
        public string Poblacion { get; set; }

        [StringLength(255)]
        public string Estado { get; set; }

        [StringLength(255)]
        public string Pais { get; set; }

        [StringLength(255)]
        public string Cp { get; set; }

        [StringLength(255)]
        public string Telefono { get; set; }

        [StringLength(255)]
        public string Estatus { get; set; }
    }
}
