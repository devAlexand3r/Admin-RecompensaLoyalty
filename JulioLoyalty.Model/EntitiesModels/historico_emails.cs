namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historico_emails
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        [Required]
        [StringLength(80)]
        public string correo_electronico { get; set; }

        public DateTime fecha_alta { get; set; }

        public Guid usuario_alta_id { get; set; }

		public string status { get; set; }
	}
}