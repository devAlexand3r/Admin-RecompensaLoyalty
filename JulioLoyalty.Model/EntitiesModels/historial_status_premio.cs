namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class historial_status_premio
    {
        public int id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal transaccion_premio_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal status_premio_id { get; set; }

        public DateTime fecha { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string observaciones { get; set; }

        public virtual status_premio status_premio { get; set; }

        public virtual transaccion_premio transaccion_premio { get; set; }
    }
}
