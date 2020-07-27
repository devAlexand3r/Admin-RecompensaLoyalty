using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.EmailService.MailChimp
{
    public class csMailChimp
    {
        private string apiKey = ConfigurationManager.AppSettings["MailChimpApiKey"].ToString();
        private string dominio = ConfigurationManager.AppSettings["dominio"].ToString();

        public CampaignSettings getSettings(string clave)
        {
            CampaignSettings settings = new CampaignSettings();
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                var query = context.mailchimp_campaignSettings.Where(m => m.clave == clave).FirstOrDefault();
                settings.FromName = query.FromName;
                settings.ReplyTo = query.ReplyTo;
                settings.Title = query.Title;
            }
            return settings;
        }

        public string getListId(string clave)
        {
            using (Model.DbContextJulio context = new Model.DbContextJulio())
            {
                var query = context.mailchimp_list.Where(m => m.clave == clave).FirstOrDefault();
                return query.listid;
            }
        }

        public async Task sendAlert(CampaignSettings settings, string listid, string plantilla, DateTime fecha_transaccion)
        {
            MailChimpManager _mailChimpManager = new MailChimpManager(apiKey);
            Setting _campaignSettings = new Setting
            {
                ReplyTo = settings.ReplyTo,
                FromName = settings.FromName,
                Title = settings.Title,
                SubjectLine = settings.SubjectLine
            };
            var content = new ContentRequest
            {
                PlainText = string.Empty,
                Html = string.Empty
            };
            plantilla = System.Web.Hosting.HostingEnvironment.MapPath(plantilla);
            using (var reader = File.OpenText(plantilla))
            {
                content.Html = reader.ReadToEnd();
                content.Html = content.Html.Replace("@dominio", dominio);
                content.Html = content.Html.Replace("@fullname", "Alejandro Jimenez");
                content.Html = content.Html.Replace("@username", "desarrollo");
                content.Html = content.Html.Replace("@password", "loyalty");
                content.Html = content.Html.Replace("@URL", "http://74.205.86.171:8051/Account/Login");
                content.Html = content.Html.Replace("@creationDate", fecha_transaccion.ToString("dd/MM/yyyy"));
            }
            try
            {
                var campaign = _mailChimpManager.Campaigns.AddAsync(new Campaign
                {
                    Settings = _campaignSettings,
                    Recipients = new Recipient { ListId = listid },
                    Type = CampaignType.Regular,
                }).Result;
                await _mailChimpManager.Content.AddOrUpdateAsync(campaign.Id.ToString(), content);
                _mailChimpManager.Campaigns.SendAsync(campaign.Id.ToString()).Wait();
            }
            catch (MailChimpException mce)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadGateway, mce.Message);
            }
            catch (Exception ex)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable, ex.Message);
            }
        }
    }
}
