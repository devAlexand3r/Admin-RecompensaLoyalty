using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequestCustomer
    {
        public int Id { get; set; }

        public string Num_tarjeta { get; set; }
        public string Nombre { get; set; }
        public string Ape_paterno { get; set; }
        public string Ape_materno { get; set; }        
        public DateTime? Fecha_nacimiento { get; set; }
        public int Genero { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }
        public string Codigo_postal { get; set; }
        public string Tel_celular { get; set; }
        public string Tel_casa { get; set; }
        public string Correo { get; set; }
        public int Ocupacion_id { get; set; }

        public int Favorito { get; set; }
        public int Frecuenta { get; set; }
        public string UserName { get; set; }

        public string URL { get; set; }// Parametro opcional
    }
}
