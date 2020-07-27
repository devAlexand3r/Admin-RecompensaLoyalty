using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada.Models
{
    public class FormLog
    {
        public decimal LogId { get; set; }

        [Required(ErrorMessage = "Nombre completo es obligatorio")]
        public string Nombre { get; set; }

        //[Required(ErrorMessage = "Debe proporcionar un número de teléfono")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "No es un número de teléfono válido")]
        //public string Telephone { get; set; }

        [Required(ErrorMessage = "Seleccione una opción")]
        public int TelefonoId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(1500, ErrorMessage = "Supero el número máximo de caracteres")]
        public string Comments { get; set; }

        [Required(ErrorMessage = "Seleccione una opción")]
        public int StatusId { get; set; }

        public List<cBase> Status { get; set; }
        public List<cTelefonos> Telefonos { get; set; }
    }
}
