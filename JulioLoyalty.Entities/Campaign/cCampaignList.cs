using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.Campaign
{
    public class cCampaignList
    {
        public class CampaignList
        {
            public string id { get; set; }
            public string campaign_id { get; set; }
            public string clave { get; set; }
            public string descripcion { get; set; }
            public string descripcion_larga { get; set; }
        }
        public List<CampaignList> lstCampaingList { get; set; }
    }
}
