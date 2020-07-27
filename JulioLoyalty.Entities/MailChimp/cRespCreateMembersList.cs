using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.MailChimp
{
	public class cRespCreateMembersList
	{
		public class MergeFields
		{
			public string ID { get; set; }
			public string MEMBRESIA { get; set; }
			public string NOMBRE { get; set; }
			public string FECHA { get; set; }
			public string CIUDAD { get; set; }
			public string ESTADO { get; set; }
			public string TIENDA { get; set; }
			public string STATUS { get; set; }
            public string TELEFONO { get; set; }
            public string NIVEL { get; set; }
        }

		public class Stats
		{
			public int avg_open_rate { get; set; }
			public int avg_click_rate { get; set; }
		}

		public class Location
		{
			public int latitude { get; set; }
			public int longitude { get; set; }
			public int gmtoff { get; set; }
			public int dstoff { get; set; }
			public string country_code { get; set; }
			public string timezone { get; set; }
		}

		public class Link
		{
			public string rel { get; set; }
			public string href { get; set; }
			public string method { get; set; }
			public string targetSchema { get; set; }
			public string schema { get; set; }
		}

		public class NewMember
		{
			public string id { get; set; }
			public string email_address { get; set; }
			public string unique_email_id { get; set; }
			public string email_type { get; set; }
			public string status { get; set; }
			public MergeFields merge_fields { get; set; }
			public Stats stats { get; set; }
			public string ip_signup { get; set; }
			public string timestamp_signup { get; set; }
			public string ip_opt { get; set; }
			public DateTime timestamp_opt { get; set; }
			public int member_rating { get; set; }
			public DateTime last_changed { get; set; }
			public string language { get; set; }
			public bool vip { get; set; }
			public string email_client { get; set; }
			public Location location { get; set; }
			public int tags_count { get; set; }
			public List<object> tags { get; set; }
			public string list_id { get; set; }
			public List<Link> _links { get; set; }
		}

		public class Error
		{
			public string email_address { get; set; }
			public string error { get; set; }
		}

		public class Link2
		{
			public string rel { get; set; }
			public string href { get; set; }
			public string method { get; set; }
			public string targetSchema { get; set; }
			public string schema { get; set; }
		}

		public class RootObject
		{
			public List<NewMember> new_members { get; set; }
			public List<object> updated_members { get; set; }
			public List<Error> errors { get; set; }
			public int total_created { get; set; }
			public int total_updated { get; set; }
			public int error_count { get; set; }
			public List<Link2> _links { get; set; }
		}
	}
}