using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Registro
{
    public class DireccionConsulta
    {
        public List<Estado> Estado { get; set; }
        public List<Municipio> Municipio { get; set; }
        public List<Colonia> Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public string Ciudad { get; set; }
    }
    public class Estado
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class Municipio
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class Colonia
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class TipoDireccion
    {
        public decimal Id { get; set; }
        public string Descripcion { get; set; }
    }
}
