namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetUsers_Participante
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ParticipanteId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual participante participante { get; set; }
    }
}
