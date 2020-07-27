using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Services
{
    public interface IRepositoryService
    {
        ResultJson UpdateRol(Parameters.RequestRol request);
        ResultJson CreateRol(Parameters.RequestRol request);

        ResultJson UpdateUserAccess(string key,string arrayKey);
        ResultJson UpdateUser(Parameters.RequesUser request);
        ResultJson CreateUser(Parameters.RequesUser request);

        ResultJson AddOrUpdateAction(Parameters.RequestAction request);
        ResultJson DeleteAction(int Id);


        ResultJson ALDistribuidor(string key, string arrayKey);

    }
}
