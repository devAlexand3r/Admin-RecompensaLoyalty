using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Transacciones
{
    public interface IRepositorioTransaccion
    {
        ResultJson AgregarTransaccion(Parameters.RequestTransaction transaction);
    }
}
