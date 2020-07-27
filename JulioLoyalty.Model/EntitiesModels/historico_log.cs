namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_log
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(100)]
        public string metodo { get; set; }

        public string parametro_principal { get; set; }

        public string parametro_producto { get; set; }

        [StringLength(250)]
        public string parametro_formapago { get; set; }

        public string respuesta { get; set; }
    }
}
