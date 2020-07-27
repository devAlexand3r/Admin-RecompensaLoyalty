using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.MailChimp
{
	public class cCampaign
	{
		public class Recipients
		{
			public string list_id { get; set; }
			public bool list_is_active { get; set; }
			public string list_name { get; set; }
			public string segment_text { get; set; }
			public int recipient_count { get; set; }
		}

		public class Settings
		{
			public string subject_line { get; set; }
			public string title { get; set; }
			public string from_name { get; set; }
			public string reply_to { get; set; }
			public bool use_conversation { get; set; }
			public string to_name { get; set; }
			public string folder_id { get; set; }
			public bool authenticate { get; set; }
			public bool auto_footer { get; set; }
			public bool inline_css { get; set; }
			public bool auto_tweet { get; set; }
			public bool fb_comments { get; set; }
			public bool timewarp { get; set; }
			public int template_id { get; set; }
			public bool drag_and_drop { get; set; }
		}

		public class Tracking
		{
			public bool opens { get; set; }
			public bool html_clicks { get; set; }
			public bool text_clicks { get; set; }
			public bool goal_tracking { get; set; }
			public bool ecomm360 { get; set; }
			public string google_analytics { get; set; }
			public string clicktale { get; set; }
		}

		public class DeliveryStatus
		{
			public bool enabled { get; set; }
		}

		public class Link
		{
			public string rel { get; set; }
			public string href { get; set; }
			public string method { get; set; }
			public string targetSchema { get; set; }
			public string schema { get; set; }
		}

		public class RootObject
		{
			public string id { get; set; }
			public int web_id { get; set; }
			public string type { get; set; }
			public DateTime create_time { get; set; }
			public string archive_url { get; set; }
			public string long_archive_url { get; set; }
			public string status { get; set; }
			public int emails_sent { get; set; }
			public string send_time { get; set; }
			public string content_type { get; set; }
			public bool needs_block_refresh { get; set; }
			public bool has_logo_merge_tag { get; set; }
			public bool resendable { get; set; }
			public Recipients recipients { get; set; }
			public Settings settings { get; set; }
			public Tracking tracking { get; set; }
			public DeliveryStatus delivery_status { get; set; }
			public List<Link> _links { get; set; }
		}
	}
}