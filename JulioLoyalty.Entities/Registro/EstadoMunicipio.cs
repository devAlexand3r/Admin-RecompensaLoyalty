using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Registro
{
    public class EstadoMunicipio
    {
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }
        public string Mensaje { get; set; }
        public List<Colonia> Colonias { get; set; }
    }
}
