namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class error_recarga_sin_saldo_suficiente
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal participante_id { get; set; }

        public DateTime fecha { get; set; }

        [Column(TypeName = "numeric")]
        public decimal transaccion_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal transaccion_premio_id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal premio_id { get; set; }

        [Required]
        [StringLength(20)]
        public string celular { get; set; }

        [Required]
        [StringLength(50)]
        public string monto { get; set; }

        [Required]
        [StringLength(50)]
        public string compania { get; set; }

        public virtual participante participante { get; set; }

        public virtual premio premio { get; set; }

        public virtual transaccion transaccion { get; set; }

        public virtual transaccion_premio transaccion_premio { get; set; }
    }
}
