namespace JulioLoyalty.Entities
{
	public class Mailing
	{
		private decimal _id;
		private string _ToMail;
		private string _ToName;
		private string _Subject;
		public decimal ID
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}
		public string ToMail
		{
			get
			{
				return _ToMail;
			}
			set
			{
				_ToMail = value;
			}
		}
		public string ToName
		{
			get
			{
				return _ToName;
			}
			set
			{
				_ToName = value;
			}
		}
		public string Subject
		{
			get
			{
				return _Subject;
			}
			set
			{
				_Subject = value;
			}
		}
	}
}
