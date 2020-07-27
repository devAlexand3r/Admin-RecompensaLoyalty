using System;
using System.Collections.Generic;
using System.Linq;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Entities.MailChimp;
using Newtonsoft.Json;
using JulioLoyalty.Model;
using System.Data;
using JulioLoyalty.Entities.Campaign;
using static JulioLoyalty.Business.Parameters.RequestGeneraLlamada;

namespace JulioLoyalty.Business.Campania
{
    public class Campania : IRepositoryCampania
    {
        private string nombre_lista;
        private string permiso_recordatorio;
        private string nombre_campania;
        private string campania_pendiente;
        private string asunto;
        private string correo_responder;
        private string nombre_responder;
        private ResultJson result = new ResultJson();
        public Campania() { }
        public string NombreLista
        {
            get
            {
                return nombre_lista;
            }
            set
            {
                nombre_lista = value;
            }
        }
        public string PermisoRecordatorio
        {
            get
            {
                return permiso_recordatorio;
            }
            set
            {
                permiso_recordatorio = value;
            }
        }
        public string NombreCampania
        {
            get
            {
                return nombre_campania;
            }
            set
            {
                nombre_campania = value;
            }
        }
        public string CampaniaPendiente
        {
            get
            {
                return campania_pendiente;
            }
            set
            {
                campania_pendiente = value;
            }
        }
        public string Asunto
        {
            get
            {
                return asunto;
            }
            set
            {
                asunto = value;
            }
        }
        public string CorreoResponder
        {
            get
            {
                return correo_responder;
            }
            set
            {
                correo_responder = value;
            }
        }
        public string NombreResponder
        {
            get
            {
                return nombre_responder;
            }
            set
            {
                nombre_responder = value;
            }
        }

        public ResultJson GeneraCampania(RequestGeneraCampania genera, string dataCenter, string apiKey, string usuario_alta_id)
        {
            try
            {
                // Crea Lista para la Campaña
                cLista.RootObject lista = CreaLista(genera, dataCenter, apiKey);
                // Obtiene el Id de la Lista Creada
                string lista_id = lista.id;
                // Hacer el MergeFields para Agregar los demas Campos Necesarios
                CreaMergeFields(dataCenter, apiKey, lista_id);
                // Agrega a los Miembros de la Lista Recien Creada
                cRespCreateMembersList.RootObject miembros = AgregarMiembros(genera, lista_id, dataCenter, apiKey);
                string campaign_id = string.Empty;
                // Crea la Campaña ó Selecciona la Campañia Pendiente sin Lista
                if (!string.IsNullOrEmpty(genera.nombre_campania) && string.IsNullOrEmpty(genera.campania_pendiente))
                {
                    // Se crea la Campaña			
                    cCampaign.RootObject campania = CreaCampania(genera, lista_id, dataCenter, apiKey);
                    campaign_id = campania.id;
                }
                else
                {
                    // Se asigna la Campaña a la Lista Recien Creada
                    bool campania = AsignaCampania(genera, lista_id, apiKey);
                    campaign_id = genera.campania_pendiente;
                }
                // Realiza la inserción en la tabla campaign
                InsertaCampania(genera, campaign_id, string.Empty, usuario_alta_id);
                // Realiza la inserción en la tabla historico_generador_campania
                InsertaHistoricoGeneradorCampania(genera, campaign_id, lista_id, usuario_alta_id);
                result.Success = true;
                result.Message = "Ha sido generada la campaña exitosamente";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ocurrio un error al generar la campaña";
                return result;
            }
        }

        private cLista.RootObject CreaLista(RequestGeneraCampania crealista, string DataCenter, string ApiKey)
        {
            // Crear una Lista
            Funciones.csMailChimp lista = new Funciones.csMailChimp();
            cLista.RootObject parsed = new cLista.RootObject();
            var sampleList = JsonConvert.SerializeObject(
                new
                {
                    name = crealista.nombre_lista,
                    contact = new
                    {
                        company = "Julio Loyalty",
                        address1 = "Matriz",
                        address2 = "CDMX",
                        city = "CDMX",
                        state = "CDMX",
                        zip = "30308",
                        country = "MX",
                        phone = ""
                    },
                    permission_reminder = crealista.permiso_recordatorio,
                    campaign_defaults = new
                    {
                        from_name = crealista.nombre_responder,
                        from_email = crealista.correo_responder,
                        subject = crealista.asunto,
                        language = "es",
                    },
                    email_type_option = true
                });
            string resp = lista.CreateList(DataCenter, sampleList, ApiKey);
            parsed = (cLista.RootObject)JsonConvert.DeserializeObject(resp, typeof(cLista.RootObject));
            return parsed;
        }

        private void CreaMergeFields(string dataCenter, string apiKey, string lista_id)
        {
            Funciones.csMailChimp lista = new Funciones.csMailChimp();
            lista.CreateMergeFields(dataCenter, apiKey, lista_id);
        }

        private cRespCreateMembersList.RootObject AgregarMiembros(RequestGeneraCampania miembros, string lista_id, string dataCenter, string apiKey)
        {
            //Llenar la Lista Recien Creada de los Miembros	
            Funciones.csMailChimp lista = new Funciones.csMailChimp();
            cRespCreateMembersList.RootObject parsed = new cRespCreateMembersList.RootObject();
            cCreateMembersList.RootObject member = new cCreateMembersList.RootObject();
            member = lista.ObtieneSocias(miembros);
            string sLista;
            if (member.members.Count() > 500)
            {
                Int32 iteracion;
                Int32 mod = member.members.Count() % 500;
                if (mod == 0)
                    iteracion = member.members.Count() / 500;
                else
                    iteracion = (member.members.Count() / 500) + 1;
                Int32 posicion = 0;
                for (int i = 0; i < iteracion; i++)
                {
                    cCreateMembersList.RootObject memberRange = new cCreateMembersList.RootObject();
                    if (posicion + 500 <= member.members.Count())
                        memberRange.members = member.members.GetRange(posicion, 500);
                    else
                        memberRange.members = member.members.GetRange(posicion, member.members.Count() - posicion);
                    memberRange.update_existing = true;
                    sLista = lista.CreateMembersList(dataCenter, apiKey, lista_id, memberRange);
                    posicion = posicion + 500;
                }
            }
            else
            {
                member.update_existing = true;
                sLista = lista.CreateMembersList(dataCenter, apiKey, lista_id, member);
                parsed = (cRespCreateMembersList.RootObject)JsonConvert.DeserializeObject(sLista, typeof(cRespCreateMembersList.RootObject));
            }
            return parsed;
        }

        private cCampaign.RootObject CreaCampania(RequestGeneraCampania genera, string lista_id, string dataCenter, string apiKey)
        {
            Funciones.csMailChimp campaign = new Funciones.csMailChimp();
            cCampaign.RootObject parsed = new cCampaign.RootObject();
            cCampaign_Recipients.RootObject recipients = new cCampaign_Recipients.RootObject();
            recipients.recipients = new cCampaign_Recipients.Recipients();
            recipients.settings = new cCampaign_Recipients.Settings();
            recipients.recipients.list_id = lista_id;
            recipients.type = "regular";
            recipients.settings.subject_line = genera.asunto;
            recipients.settings.reply_to = genera.correo_responder;
            recipients.settings.from_name = genera.nombre_responder;
            string resp = campaign.CreateCampaign(dataCenter, apiKey, recipients);
            parsed = (cCampaign.RootObject)JsonConvert.DeserializeObject(resp, typeof(cCampaign.RootObject));
            return parsed;
        }

        private bool AsignaCampania(RequestGeneraCampania genera, string lista_id, string apiKey)
        {
            Funciones.csMailChimp campaign = new Funciones.csMailChimp();
            cCampaign_Recipients.RootObject recipients = new cCampaign_Recipients.RootObject();
            recipients.recipients = new cCampaign_Recipients.Recipients();
            recipients.settings = new cCampaign_Recipients.Settings();
            recipients.recipients.list_id = lista_id;
            recipients.type = "regular";
            recipients.settings.subject_line = genera.asunto;
            recipients.settings.reply_to = genera.correo_responder;
            recipients.settings.from_name = genera.nombre_responder;
            bool resp = campaign.UpdateCampaign(apiKey, genera, recipients);
            return resp;
        }

        private RequestGeneraCampania InsertaCampania(RequestGeneraCampania genera, string campaign_id, string script, string usuario_alta_id)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@campaign_id", campaign_id);
                    parameters.Add("@clave", campaign_id);
                    parameters.Add("@descripcion", genera.nombre_campania);
                    parameters.Add("@descripcion_larga", genera.nombre_campania);
                    parameters.Add("@script", script);
                    parameters.Add("@usuario_alta_id", usuario_alta_id);
                    DataTable dt = db.GetDataSet("[dbo].[usp_Inserta_Campaign]", CommandType.StoredProcedure, parameters).Tables[0];
                    if (dt.Rows.Count > 0)
                        genera.id = dt.Rows[0]["id"].ToString();
                }
                return genera;
            }
            catch (Exception ex)
            {
                return genera;
            }
        }

        private bool ActualizaScriptCampania(RequestGeneraLlamada genera, string usuario_id)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@id", genera.campania_llamada);
                    parameters.Add("@script", genera.script);
                    parameters.Add("@usuario_cambio_id", usuario_id);
                    db.GetDataSet("[dbo].[usp_Actualiza_Campaign_Script]", CommandType.StoredProcedure, parameters);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool InsertaHistoricoGeneradorCampania(RequestGeneraCampania genera, string campaign_id, string list_id, string usuario_alta_id)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    foreach (var item in genera.miembros)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("@participante_id", item.hidden);
                        parameters.Add("@campaign_id", campaign_id);
                        parameters.Add("@nombre_campania", genera.nombre_campania);
                        parameters.Add("@list_id", list_id);
                        parameters.Add("@nombre_lista", genera.nombre_lista);
                        parameters.Add("@asunto", genera.asunto);
                        parameters.Add("@usuario_alta_id", usuario_alta_id);
                        db.GetDataSet("[dbo].[usp_Inserta_historico_generador_campania]", CommandType.StoredProcedure, parameters);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool InsertaParticipanteCampania(RequestGeneraLlamada genera)
        {
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    foreach (var itemUsuario in genera.usuarios)
                    {
                        int cont = 0;
                        var id = itemUsuario.Id;
                        int llamada = int.Parse(itemUsuario.Llamada);
                        foreach (var x in genera.miembros.Where(x => x.usuario_id == null)) // Solo toma los no asignados al usuario
                        {
                            if (cont < llamada)
                            {
                                var participante_id = x.hidden;
                                var clave = x.clave;
                                var nombre = x.nombre;
                                var usuario_id = id;
                                x.usuario_id = usuario_id;
                                // inserta participante_campania
                                Dictionary<string, object> parameters = new Dictionary<string, object>();
                                parameters.Add("@campania_id", genera.id);
                                parameters.Add("@participante_id", participante_id);
                                parameters.Add("@clave", clave);
                                parameters.Add("@nombre_completo", nombre);
                                parameters.Add("@usuario_id", usuario_id);
                                db.GetDataSet("[dbo].[usp_Inserta_Participante_Campaña]", CommandType.StoredProcedure, parameters);
                                cont++;
                            }
                            else
                                break;
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

        public cUsuariosLlamadas consultaUsuariosLlamadas()
        {
            cUsuariosLlamadas usuarios_llamadas = new cUsuariosLlamadas();
            usuarios_llamadas.lstUsuariosLlamadas = new List<cUsuariosLlamadas.UsuariosLlamadas>();
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters = null;
                    DataSet dsConsulta = db.GetDataSet("[dbo].[usp_Consulta_Users_Roles_Admin_Tienda]", CommandType.StoredProcedure, parameters);
                    foreach (DataRow item in dsConsulta.Tables[0].Rows)
                    {
                        usuarios_llamadas.lstUsuariosLlamadas.Add(new cUsuariosLlamadas.UsuariosLlamadas
                        {
                            id = item["id"].ToString(),
                            Email = item["Email"].ToString(),
                            UserName = item["UserName"].ToString(),
                            Role = item["Role"].ToString(),
                            Llamada = item["Llamada"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return usuarios_llamadas;
        }

        public cCampaignList consultaCampanias()
        {
            cCampaignList campania = new cCampaignList();
            campania.lstCampaingList = new List<cCampaignList.CampaignList>();
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters = null;
                    DataSet dsConsulta = db.GetDataSet("[dbo].[usp_Consulta_Campaign]", CommandType.StoredProcedure, parameters);
                    foreach (DataRow item in dsConsulta.Tables[0].Rows)
                    {
                        campania.lstCampaingList.Add(new cCampaignList.CampaignList
                        {
                            id = item["id"].ToString(),
                            campaign_id = item["campaign_id"].ToString(),
                            clave = item["clave"].ToString(),
                            descripcion = item["descripcion"].ToString(),
                            descripcion_larga = item["descripcion_larga"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return campania;
        }

        public ResultJson GeneraLlamada(RequestGeneraLlamada genera, string usuario_alta_id)
        {
            try
            {
                RequestGeneraCampania inserta_campania = new RequestGeneraCampania();
                if (genera.campania_llamada == null) // Va a Insertar en Tabla Campaign
                {
                    inserta_campania.nombre_campania = genera.nombre_campania;
                    InsertaCampania(inserta_campania, string.Empty, genera.script, usuario_alta_id);
                }
                else // Va actualizar el script de la tabla Campaign
                {
                    ActualizaScriptCampania(genera, usuario_alta_id);
                }
                // Inserta en la tabla participante_campaña
                if (inserta_campania.id != null) // Se dio de alta una campaña
                    genera.id = inserta_campania.id;
                else
                    genera.id = genera.campania_llamada; // Se obtiene el id de la Campaña del Combo Seleccionado
                InsertaParticipanteCampania(genera);
                result.Success = true;
                result.Message = "Ha sido generada la llamada exitosamente";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ocurrio un error al generar la llamadacampaña";
                return result;
            }
        }


        public ResultJson SaveSegment(string prDescription, string prLongDescription, string prSQL, string userId)
        {
            var prResult = new ResultJson()
            {
                Message = "Segmento ha sido agregado correctamente",
                Success = true
            };

            if (string.IsNullOrEmpty(prDescription) || string.IsNullOrEmpty(prLongDescription) || string.IsNullOrEmpty(prSQL))
            {
                prResult.Message = "Parametros incorrectos";
                prResult.Success = false;
            }
            else
            {
                Dictionary<string, object> prSegment = new Dictionary<string, object>() {
                    { "descripcion", prDescription},
                    { "descripcion_larga", prLongDescription },
                    { "usuario_id", userId}
                };
                Dictionary<string, object> prQueryBuilder = new Dictionary<string, object>() {
                    { "sql", prSQL}
                };
                int segmento_id = 0;
                //Continuar...
                using (DbContextJulio dbContext = new DbContextJulio())
                {
                    try
                    {
                        DataTable dtSegment = dbContext.GetDataSet("[dbo].[usp_inserta_segmento]", CommandType.StoredProcedure, prSegment).Tables[0];
                        foreach (DataRow row in dtSegment.Rows)
                        {
                            if (dtSegment.Columns.Contains("id"))
                                segmento_id = Convert.ToInt32(row["id"]);
                        }

                        if (segmento_id > 0)
                        {
                            decimal participante_id = 0;
                            DataTable dtResult = dbContext.GetDataSet("[dbo].[usp_queryBuilder_result]", CommandType.StoredProcedure, prQueryBuilder).Tables[0];
                            foreach (DataRow row in dtResult.Rows)
                            {
                                participante_id = Convert.ToDecimal(row[0]);
                                Dictionary<string, object> prCurrent = new Dictionary<string, object>() {
                                    { "segmento_id", segmento_id },
                                    { "participante_id", participante_id }
                                };
                                var result = dbContext.GetDataSet("[dbo].[usp_inserta_segmento_participante]", CommandType.StoredProcedure, prCurrent).Tables[0];
                            }
                        }
                        else
                        {
                            prResult.Message = "Vuelva a intentarlo";
                            prResult.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        prResult.Message = ex.Message.ToString();
                        prResult.Success = false;
                    }                   
                }
            }
            return prResult;
        }
    }
}