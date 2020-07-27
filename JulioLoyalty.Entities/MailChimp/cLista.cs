using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.MailChimp
{
	public class cLista
	{
		public class Contact
		{
			public string company { get; set; }
			public string address1 { get; set; }
			public string address2 { get; set; }
			public string city { get; set; }
			public string state { get; set; }
			public string zip { get; set; }
			public string country { get; set; }
			public string phone { get; set; }
		}

		public class CampaignDefaults
		{
			public string from_name { get; set; }
			public string from_email { get; set; }
			public string subject { get; set; }
			public string language { get; set; }
		}

		public class Stats
		{
			public int member_count { get; set; }
			public int unsubscribe_count { get; set; }
			public int cleaned_count { get; set; }
			public int member_count_since_send { get; set; }
			public int unsubscribe_count_since_send { get; set; }
			public int cleaned_count_since_send { get; set; }
			public int campaign_count { get; set; }
			public string campaign_last_sent { get; set; }
			public int merge_field_count { get; set; }
			public int avg_sub_rate { get; set; }
			public int avg_unsub_rate { get; set; }
			public int target_sub_rate { get; set; }
			public double open_rate { get; set; }
			public int click_rate { get; set; }
			public string last_sub_date { get; set; }
			public string last_unsub_date { get; set; }
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
			public string name { get; set; }
			public Contact contact { get; set; }
			public string permission_reminder { get; set; }
			public bool use_archive_bar { get; set; }
			public CampaignDefaults campaign_defaults { get; set; }
			public string notify_on_subscribe { get; set; }
			public string notify_on_unsubscribe { get; set; }
			public DateTime date_created { get; set; }
			public int list_rating { get; set; }
			public bool email_type_option { get; set; }
			public string subscribe_url_short { get; set; }
			public string subscribe_url_long { get; set; }
			public string beamer_address { get; set; }
			public string visibility { get; set; }
			public bool double_optin { get; set; }
			public bool has_welcome { get; set; }
			public bool marketing_permissions { get; set; }
			public List<object> modules { get; set; }
			public Stats stats { get; set; }
			public List<Link> _links { get; set; }
		}
	}
}