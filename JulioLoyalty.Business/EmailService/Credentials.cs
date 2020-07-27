using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.EmailService
{
    public class Credentials
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }

        public string Copy { get; set; }
        public string CopyHidden { get; set; }

        public Credentials()
        {
            this.Host = "secure.emailsrvr.com";
            this.UserName = "contacto@julioloyalty.com";
            this.Password = "Gp0Jul1oCont4ct0.";
            this.DisplayName = "Contacto JULIO Loyalty";
            this.Address = "contacto@julioloyalty.com";
            this.Subject = "Contacto Loyalty";
            this.CopyHidden = "alejandro.jimenez.26@outlook.com";
        }
    }
}
