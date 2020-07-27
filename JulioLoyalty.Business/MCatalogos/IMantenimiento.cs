using JulioLoyalty.Business.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.MCatalogos
{
    public interface IMantenimiento
    {
        ResultJson AgregarMantenimiento(SQLParameters parameters);
        ResultJson ActualizarMantenimiento(SQLParameters parameters);
        ResultJson EliminarCEMantenimiento(SQLParameters parameters);
    }
}
