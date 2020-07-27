using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.EmailService
{
    public interface IRepositoryEmail
    {
        void Submit();
        void SubmitExit();
        void UnifySubmitExit();
    }
}
