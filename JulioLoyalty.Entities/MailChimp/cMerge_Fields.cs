using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.MailChimp
{
	public class cMerge_Fields
	{
		public string EMAIL { get; set; }

        //ID ECOMM
        public string IDECOMM { get; set; }

		public string NOMECOMM { get; set; }

        //APELLIDO(S) ECOMM
        public string APEECOMM { get; set; }

        //ESTADO ECOMM
        public string ESTECOMM { get; set; }

        //ULTIMA COMPRA ECOMM
        public string COMPECOMM { get; set; }

		public string MONTECOMM { get; set; }

		public string MOVIL1 { get; set; }

        //ID LOYALTY
        public string IDLOYALTY { get; set; }
        
        public string NOMBRE { get; set; }

        //ID LEAD
        public string MMERGE4 { get; set; }

        //APELLIDO(S) SOCIA
        public string APESOCIA { get; set; }

		public string SUCURSAL { get; set; }

		public string NUMSOCIA { get; set; }

        //ESTADO SOCIA
        public string ESTSOCIA { get; set; }

        //FECHA DE NACIMIENTO
        public string MMERGE18 { get; set; }

        //ID FACTURAS
        public string MMERGE14 { get; set; }
    }
}