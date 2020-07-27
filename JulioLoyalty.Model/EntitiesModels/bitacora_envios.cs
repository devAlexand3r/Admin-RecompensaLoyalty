namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bitacora_envios
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string correo_electronico { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(255)]
        public string asunto { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(4000)]
        public string mensaje { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime fecha { get; set; }

        [Key]
        [Column(Order = 6)]
        public Guid usuario_id { get; set; }
    }
}
