using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.EmailService
{
    public class EParameters
    {
        public int IdUnique { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public DateTime currentTime { get; set; }


        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
