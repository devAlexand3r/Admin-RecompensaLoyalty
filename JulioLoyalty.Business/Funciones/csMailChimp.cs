using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Entities.MailChimp;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using JulioLoyalty.Business.Parameters;
using System.Reflection;
using MailChimp.Net;
using MailChimp.Net.Models;
using MailChimp.Net.Core;
using static JulioLoyalty.Entities.MailChimp.cCampaignWithoutList;
using System.Data;
using JulioLoyalty.Model;

namespace JulioLoyalty.Business.Funciones
{
    public class csMailChimp
    {
        // Agregar o Actualizar un Subscriptor
        public string AddOrUpdateListMember(string dataCenter, string apiKey, string listId, cMerge_Fields fields, string status, string subscriberEmail)
        {
            var sampleListMember = JsonConvert.SerializeObject(
                new
                {
                    email_address = fields.EMAIL, // correo_electronico a cambiar
                    merge_fields =
                    new
                    {
                        NUMSOCIA = fields.NUMSOCIA,
                        NOMBRE = fields.NOMBRE,
                        APESOCIA = fields.APESOCIA,
                        MMERGE18 = fields.MMERGE18, // Fecha de Nacimiento                        
                        MOVIL1 = fields.MOVIL1,
                        ESTSOCIA = fields.ESTSOCIA
                    },
                    status_if_new = status
                });

            var hashedEmailAddress = string.IsNullOrEmpty(subscriberEmail) ? "" : CalculateMD5Hash(subscriberEmail.ToLower()); // correo_electronico actual con hash
            var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists/{1}/members/{2}", dataCenter, listId, hashedEmailAddress);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);
                    webClient.Encoding = Encoding.UTF8;
                    return webClient.UploadString(uri, "PUT", sampleListMember);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static string CalculateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input.
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string.
            var sb = new StringBuilder();
            foreach (var @byte in hash)
            {
                sb.Append(@byte.ToString("X2"));
            }
            return sb.ToString();
        }

        // Obtener Todas las Campañas
        public string GetListCampaign(string dataCenter, string apiKey)
        {
            //https://us12.api.mailchimp.com/3.0/campaigns?apikey=214027fb77c96df79ed50c560fc1528a-us12
            var uri = string.Format("https://{0}.api.mailchimp.com/3.0/campaigns", dataCenter);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);
                    webClient.Encoding = Encoding.UTF8;
                    return webClient.DownloadString(uri);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public Int32 GetCampaignCount(string dataCenter, string apiKey)
        {
            string resp = GetListCampaign(dataCenter, apiKey);
            cListCampaign.RootObject parsed = new cListCampaign.RootObject();
            parsed = (cListCampaign.RootObject)JsonConvert.DeserializeObject(resp, typeof(cListCampaign.RootObject));
            Int32 count = parsed.total_items;
            return count;
        }

        public string GetCampaignStatusList(string dataCenter, string apiKey, string status, Int32 count, Int32 offset)
        {
            var uri = string.Format("https://{0}.api.mailchimp.com/3.0/campaigns?status={1}&count={2}&offset={3}", dataCenter, status, count, offset);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);
                    webClient.Encoding = Encoding.UTF8;
                    return webClient.DownloadString(uri);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        // Crear una Lista
        public string CreateList(string dataCenter, string sampleList, string apiKey)
        {
            var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists", dataCenter);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);
                    webClient.Encoding = Encoding.UTF8;
                    return webClient.UploadString(uri, "POST", sampleList);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        // Agregar MergeFields a la Lista
        public void CreateMergeFields(string dataCenter, string apiKey, string ListId)
        {
            var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists/{1}/merge-fields", dataCenter, ListId);
            Type type = typeof(cCreateMembersList.MergeFields);
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                cCreateMembersList.MergeFields.CreateMergeFields mergefields = new cCreateMembersList.MergeFields.CreateMergeFields();
                mergefields.name = propertyInfo.Name;
                mergefields.type = "text";
                mergefields.Public = true;
                mergefields.tag = propertyInfo.Name;
                var jsonString = JsonConvert.SerializeObject(mergefields);
                jsonString = jsonString.ToLower();
                try
                {
                    using (var webClient = new WebClient())
                    {
                        webClient.Headers.Add("Accept", "application/json");
                        webClient.Headers.Add("Authorization", "apikey " + apiKey);
                        webClient.Encoding = Encoding.UTF8;
                        webClient.UploadString(uri, jsonString);
                    }
                }
                catch (WebException we)
                {
                    using (var sr = new StreamReader(we.Response.GetResponseStream()))
                    {
                        sr.ReadToEnd();
                    }
                }
            }
        }

        // Subscribir o Desusbcribir una Lista de Miembros
        public string CreateMembersList(string dataCenter, string apiKey, string ListId, cCreateMembersList.RootObject member)
        {
            var uri = string.Format("https://{0}.api.mailchimp.com/3.0/lists/{1}", dataCenter, ListId);
            var jsonString = JsonConvert.SerializeObject(member);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);
                    webClient.Encoding = Encoding.UTF8;
                    return webClient.UploadString(uri, jsonString);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        // De la Lista de Miembros se asigna a la Clase para su Lectura
        public cCreateMembersList.RootObject ObtieneSocias(RequestGeneraCampania miembros)
        {
            cCreateMembersList.RootObject member = new cCreateMembersList.RootObject();
            List<cCreateMembersList.Member> lstMember = new List<cCreateMembersList.Member>();
            // Aqui se van a omitir los miembros sin correo y únicos
            var uniqueMiembros = from p in miembros.miembros
                                 where !string.IsNullOrEmpty(p.correo)
                                 group p by new { p.correo }
                               into grupoMiembros
                                 select grupoMiembros.FirstOrDefault();
            foreach (var item in uniqueMiembros)
            {
                lstMember.Add(new cCreateMembersList.Member
                {
                    email_address = item.correo,
                    status = "subscribed",
                    status_if_new = "subscribed",
                    merge_fields = new cCreateMembersList.MergeFields()
                    {
                        ID = item.hidden,
                        MEMBRESIA = item.clave,
                        NOMBRE = item.nombre,
                        FECHA = item.fecha,
                        CIUDAD = item.ciudad,
                        ESTADO = item.estado,
                        TIENDA = item.tienda,
                        STATUS = item.status,
                        TELEFONO = item.telefono,
                        NIVEL = item.nivel
                    }
                });
            }
            member.members = lstMember;
            return member;
        }

        // Crear Una Campaña
        public string CreateCampaign(string dataCenter, string apiKey, cCampaign_Recipients.RootObject recipients)
        {
            var uri = string.Format("https://{0}.api.mailchimp.com/3.0/campaigns", dataCenter);
            var jsonString = JsonConvert.SerializeObject(recipients);
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Accept", "application/json");
                    webClient.Headers.Add("Authorization", "apikey " + apiKey);
                    webClient.Encoding = Encoding.UTF8;
                    return webClient.UploadString(uri, jsonString);
                }
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        // Actualizar Una Campaña		
        public bool UpdateCampaign(string apiKey, RequestGeneraCampania genera, cCampaign_Recipients.RootObject recipients)
        {
            MailChimpManager mgr = new MailChimpManager(apiKey);
            try
            {
                Campaign newCampaign = new Campaign();
                newCampaign.Id = genera.campania_pendiente;
                newCampaign.Type = CampaignType.Regular;
                newCampaign.Settings = new Setting();
                newCampaign.Settings.SubjectLine = recipients.settings.subject_line;
                newCampaign.Recipients = new Recipient();
                newCampaign.Recipients.ListId = recipients.recipients.list_id;
                newCampaign.Settings.FromName = genera.nombre_responder;
                newCampaign.Settings.ReplyTo = genera.correo_responder;
                newCampaign = mgr.Campaigns.AddOrUpdateAsync(newCampaign).Result;
                genera.campania_pendiente = newCampaign.Id;
                return (!String.IsNullOrWhiteSpace(genera.campania_pendiente));
            }
            finally
            {
                mgr = null;
            }
        }

        public bool UpdateTableCampaign(List<CampaignWithoutList> lstCampaingWithoutList)
        {
            try
            {
                foreach (var item in lstCampaingWithoutList)
                {
                    // Agregar Campañas Pendientes a la Tabla campaigns, Provenientes de la lista de Campañas Pendientes de MailChimp
                    insert_update_delete_Campaign(item.campaign_id, item.name, "A");
                }
                DataTable dtConsulta = ConsultaCampaniaPendientes();
                if (dtConsulta != null)
                {
                    foreach(DataRow row in dtConsulta.Rows)
                    {
                        // Ocupa para buscar las campañas pendientes que ya no se encuentran en la lista de Campañas Mailchimp
                        // Si ya no se encuentra darla de baja en al tabla campaigns
                        if (!lstCampaingWithoutList.Exists(b => b.campaign_id == row["campaign_id"].ToString()))
                        {
                            insert_update_delete_Campaign(row["campaign_id"].ToString(), row["campaign_id"].ToString(), "B");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void insert_update_delete_Campaign(string campaign_id, string name, string opcion)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@campaign_id", campaign_id);
                parameters.Add("@name", name);
                parameters.Add("@opcion", opcion);
                DataSet setTables = db.GetDataSet("[dbo].[usp_insert_update_delete_campaign]", CommandType.StoredProcedure, parameters);
            }
        }

        private DataTable ConsultaCampaniaPendientes()
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                DataSet setTables = db.GetDataSet("[dbo].[usp_consulta_campaign_pendientes]", CommandType.StoredProcedure, null);
                DataTable dtConsulta = setTables.Tables[0];
                return dtConsulta;
            }
        }
    }
}