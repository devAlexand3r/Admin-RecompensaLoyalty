using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Mecanica
{
    public class MecanicaNivel
    {
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [StringLength(250, ErrorMessage = "El nombre debe tener una longitud mínima de 1 y máxima de 250")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Porcentaje es requerido")]
        [Range(0, 100, ErrorMessage = "Introduzca un número entre 0 y 100")]
        public double? Porcentaje { get; set; }

        [Required(ErrorMessage = "Rango inicial es requerido")]
        public double? MontoMinimo { get; set; }

        [Required(ErrorMessage = "Rango final es requerido")]
        public double? MontoMaximo { get; set; }
    }
}
