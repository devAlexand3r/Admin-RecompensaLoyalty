using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Entities.ValidaCorreo
{
	public class csValidaCorreo
	{
		public class ValidaCorreo
		{
			public string ErrorException { get; set; }
			public Version Version { get; set; }
			public Meta Meta { get; set; }
			public Disposition Disposition { get; set; }
			public SyntaxVerification SyntaxVerification { get; set; }
			public RecordRoot RecordRoot { get; set; }
			public RecordWww RecordWww { get; set; }
			public MxRecord MxRecord { get; set; }
			public DnsVerification DnsVerification { get; set; }
			public MailboxVerification MailboxVerification { get; set; }
			public EmailVerification EmailVerification { get; set; }
			public Mail Mail { get; set; }
			public Web Web { get; set; }
			public Infrastructure Infrastructure { get; set; }
			public SendAssess SendAssess { get; set; }
			public BlockList BlockList { get; set; }
			public SpamAssess SpamAssess { get; set; }
			public SpamTrapAssess SpamTrapAssess { get; set; }
			public HippoTrust HippoTrust { get; set; }
			public Gravatar Gravatar { get; set; }
			public Social Social { get; set; }
			public Performance Performance { get; set; }
			public Diagnostic Diagnostic { get; set; }
			public RootObject RootObject { get; set; }
		}

		public class Version
		{
			public string v { get; set; }
			public string doc { get; set; }
		}

		public class Meta
		{
			public string lastModified { get; set; }
			public string expires { get; set; }
			public string email { get; set; }
			public string tld { get; set; }
			public string domain { get; set; }
			public object subDomain { get; set; }
			public string user { get; set; }
			public string emailHashMd5 { get; set; }
			public string emailHashSha1 { get; set; }
			public string emailHashSha256 { get; set; }
		}

		public class Disposition
		{
			public bool isRole { get; set; }
			public bool isFreeMail { get; set; }
		}

		public class SyntaxVerification
		{
			public bool isSyntaxValid { get; set; }
			public string reason { get; set; }
		}

		public class RecordRoot
		{
			public List<string> ipAddresses { get; set; }
		}

		public class RecordWww
		{
			public List<string> ipAddresses { get; set; }
		}

		public class MxRecord
		{
			public int preference { get; set; }
			public string exchange { get; set; }
			public List<string> ipAddresses { get; set; }
		}

		public class DnsVerification
		{
			public bool isDomainHasDnsRecord { get; set; }
			public bool isDomainHasMxRecords { get; set; }
			public RecordRoot recordRoot { get; set; }
			public RecordWww recordWww { get; set; }
			public List<MxRecord> mxRecords { get; set; }
			public List<string> txtRecords { get; set; }
		}

		public class MailboxVerification
		{
			public string result { get; set; }
			public string reason { get; set; }
		}

		public class EmailVerification
		{
			public SyntaxVerification syntaxVerification { get; set; }
			public DnsVerification dnsVerification { get; set; }
			public MailboxVerification mailboxVerification { get; set; }
		}

		public class Mail
		{
			public string serviceTypeId { get; set; }
			public string mailServerLocation { get; set; }
			public string smtpBanner { get; set; }
		}

		public class Web
		{
			public bool hasAliveWebServer { get; set; }
		}

		public class Infrastructure
		{
			public Mail mail { get; set; }
			public Web web { get; set; }
		}

		public class SendAssess
		{
			public double inboxQualityScore { get; set; }
			public string sendRecommendation { get; set; }
		}

		public class BlockList
		{
			public string blockListName { get; set; }
			public bool isListed { get; set; }
			public object listedReason { get; set; }
			public object listedMoreInfo { get; set; }
		}

		public class SpamAssess
		{
			public bool isDisposableEmailAddress { get; set; }
			public bool isDarkWebEmailAddress { get; set; }
			public bool isGibberishDomain { get; set; }
			public bool isGibberishUser { get; set; }
			public double domainRiskScore { get; set; }
			public double formatRiskScore { get; set; }
			public double profanityRiskScore { get; set; }
			public double overallRiskScore { get; set; }
			public string actionRecomendation { get; set; }
			public List<BlockList> blockLists { get; set; }
		}

		public class SpamTrapAssess
		{
			public bool isSpamTrap { get; set; }
			public object spamTrapDescriptor { get; set; }
		}

		public class HippoTrust
		{
			public double score { get; set; }
			public string level { get; set; }
		}

		public class Gravatar
		{
			public string imageUrl { get; set; }
			public string profileUrl { get; set; }
		}

		public class Social
		{
			public Gravatar gravatar { get; set; }
		}

		public class Performance
		{
			public int syntaxCheck { get; set; }
			public int dnsLookup { get; set; }
			public int spamAssessment { get; set; }
			public int mailboxVerification { get; set; }
			public int webInfrastructurePing { get; set; }
			public int other { get; set; }
			public int overallExecutionTime { get; set; }
		}

		public class Diagnostic
		{
			public string key { get; set; }
		}

		public class RootObject
		{
			public Version version { get; set; }
			public Meta meta { get; set; }
			public Disposition disposition { get; set; }
			public EmailVerification emailVerification { get; set; }
			public Infrastructure infrastructure { get; set; }
			public SendAssess sendAssess { get; set; }
			public SpamAssess spamAssess { get; set; }
			public SpamTrapAssess spamTrapAssess { get; set; }
			public HippoTrust hippoTrust { get; set; }
			public Social social { get; set; }
			public object domain { get; set; }
			public Performance performance { get; set; }
			public Diagnostic diagnostic { get; set; }
		}
	}
}
