using JulioLoyalty.Business.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business
{
	public interface IRepositoryCampania
	{
		ResultJson GeneraCampania(RequestGeneraCampania genera, string dataCenter, string apiKey, string usuario_alta_id);
        ResultJson GeneraLlamada(RequestGeneraLlamada genera, string usuario_alta_id);
    }
}