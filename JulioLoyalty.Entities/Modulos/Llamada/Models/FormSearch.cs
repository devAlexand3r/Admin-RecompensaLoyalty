using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Modulos.Llamada.Models
{
    public class FormSearch
    {
        [Required(ErrorMessage = "Por favor, ingrese algún parámetro valido")]
        [MaxLength(256, ErrorMessage = "Supero el número máximo de caracteres")]
        public string Parameters { get; set; }

        public decimal? Campaign_id { get; set; }

        public List<cBase> Campaigns { get; set; }

        public List<cBusqueda> SearchResult { get; set; }

    }
}
