using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.MailChimp
{
	public class cCampaign_Recipients
	{
		public class Recipients
		{
			public string list_id { get; set; }
		}

		public class Settings
		{
			public string subject_line { get; set; }
			public string reply_to { get; set; }
			public string from_name { get; set; }
		}

		public class RootObject
		{
			public Recipients recipients { get; set; }
			public string type { get; set; }
			public Settings settings { get; set; }
		}
	}
}