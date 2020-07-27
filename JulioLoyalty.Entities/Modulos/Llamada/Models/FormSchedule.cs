using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada.Models
{
    public class FormSchedule
    {
        public decimal ScheduleId { get; set; }

        [Required(ErrorMessage = "Nombre completo es requerido")]
        public string Nombre { get; set; }

        public int PhoneNumberId { get; set; }

        [Required(ErrorMessage = "Fecha (dd/mm/yyyy) es requerido")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Hora (hh:mm) es requerido")]   
        [RegularExpression(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato inválido")]
        public string Hours { get; set; }

        public DateTime FullDate { get; set; }

        [Required(ErrorMessage = "Comentarios es requerido")]
        public string Comments { get; set; }

        //[RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9]$", ErrorMessage = "Formato inválido")]
    }
}
