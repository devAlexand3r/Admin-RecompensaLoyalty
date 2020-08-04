using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Model.EntitiesModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Configuracion
{
    public interface IConfiguracion
    {
        Task<RequestPais> GetPaisAsync();
        Task<List<tema>> GetTemaListAsync();
        Task<RequestPais> UpdatePaisAsync(RequestPais pais);
    }
}