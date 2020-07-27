using JulioLoyalty.Business.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Llamada
{
	public interface IRepositoryLlamada
	{
		ResultJson Envio(RequestEnvio envio);
		ResultJson Captura(RequestCaptura captura);
		ResultJson Seguimiento(RequestSeguimiento seguimiento);
	}
}
