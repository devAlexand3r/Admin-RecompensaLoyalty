namespace JulioLoyalty.Model.EntitiesModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_tickets
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tmp_tickets()
        {
            tmp_tickets_formasdepago = new HashSet<tmp_tickets_formasdepago>();
            tmp_tickets_productos = new HashSet<tmp_tickets_productos>();
        }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [StringLength(255)]
        public string NoTicket { get; set; }

        [StringLength(255)]
        public string ClvSucursal { get; set; }

        [StringLength(255)]
        public string FechaCompra { get; set; }

        public double? MontoSD { get; set; }

        public double? MontoCD { get; set; }

        public int? NumArticulos { get; set; }

        [StringLength(255)]
        public string Tarjeta { get; set; }

        [StringLength(255)]
        public string TicketDevolucion { get; set; }

        [StringLength(255)]
        public string Cancela { get; set; }

        [StringLength(255)]
        public string Cajero { get; set; }

        [StringLength(50)]
        public string Foliosbx { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tmp_tickets_formasdepago> tmp_tickets_formasdepago { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tmp_tickets_productos> tmp_tickets_productos { get; set; }
    }
}
