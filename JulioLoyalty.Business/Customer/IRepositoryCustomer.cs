using JulioLoyalty.Business.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Customer
{
    public interface IRepositoryCustomer
    {
        ResultJson AddCustomer(RequestCustomer customer);
        ResultJson UpdateCustomer(RequestCustomer customer);
        ResultJson UnifyCustomer(RequestCustomer customer);

        ResultJson AddUser(RequesUser user);
        ResultJson UpdateUser(RequesUser user);

        ResultJson UpdateStutus(RequestComments comm);
        decimal ObtieneStatusPartipanteId(RequestCustomer customer);
        bool ActualizaStatusParticipante(RequestCustomer customer, decimal status_participante_id);
        Task<ResultJson> ThemeAsync(string theme);
    }
}