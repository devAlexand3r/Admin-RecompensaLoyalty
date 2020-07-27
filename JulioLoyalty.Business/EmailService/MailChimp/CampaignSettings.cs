using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.EmailService.MailChimp
{
    public class CampaignSettings
    {
        public string ReplyTo { get; set; }
        public string FromName { get; set; }
        public string Title { get; set; }
        public string SubjectLine { get; set; }
    }
}
