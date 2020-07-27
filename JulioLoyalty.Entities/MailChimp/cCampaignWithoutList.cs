using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.MailChimp
{
	public class cCampaignWithoutList
	{
		public class CampaignWithoutList
		{
			public string campaign_id { get; set; }
			public string name { get; set; }
		}
		public List<CampaignWithoutList> lstCampaingWithoutList { get; set; }
	}
}