using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.MailChimp
{
	public class cCreateMembersList
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
            public class CreateMergeFields
			{
				public string name { get; set; }
				public string type { get; set; }
				public bool Public { get; set; }
				public string tag { get; set; }				
			}
		}

		public class Member
		{
			public string email_address { get; set; }
			public string status { get; set; }
			public string status_if_new { get; set; }
			public MergeFields merge_fields { get; set; }
		}

		public class RootObject
		{
			public List<Member> members { get; set; }
			public bool update_existing { get; set; }
		}
	}
}