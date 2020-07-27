namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mailing")]
    public partial class mailing
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(100)]
        public string FromMail { get; set; }

        [Required]
        [StringLength(100)]
        public string FromName { get; set; }

        [Required]
        [StringLength(100)]
        public string ToMail { get; set; }

        [Required]
        [StringLength(100)]
        public string ToName { get; set; }

        [Required]
        [StringLength(150)]
        public string Subject { get; set; }

        [Required]
        [StringLength(20)]
        public string pais { get; set; }
    }
}
