namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class transaccion_comentarios
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal transaccion_id { get; set; }

        public string comentarios { get; set; }

        public DateTime? fecha_alta { get; set; }

        public Guid? usuario_alta_id { get; set; }

        public virtual transaccion transaccion { get; set; }
    }
}
