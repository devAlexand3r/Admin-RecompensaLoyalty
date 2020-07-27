using JulioLoyalty.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Entities.Modulos.Llamada;
using System.Data.SqlClient;
using JulioLoyalty.Entities.Modulos;

namespace JulioLoyalty.Business
{
    public class UnitOfWork
    {
        public static List<Dictionary<string, object>> EveryWeek(string userId)
        {
            #region Parameter SQL
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);
            #endregion        
            DataTable table = Execute.StoreProcedure("[dbo].[usp_cubo_participante_distribuidor]", parameters).Tables[0];
            return ConvertTo.TableToList(table);
        }

        public static List<Dictionary<string, object>> BuyDataResult(DateTime dateStart, DateTime dateEnd, int periodo_id = 1)
        {
            #region Parameter SQL
            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@periodo_id", periodo_id },
                { "@fecha_inicial", dateStart},
                { "@fecha_final", dateEnd}
            };
            #endregion 
            DataTable table = Execute.StoreProcedure("[dbo].[usp_cubo_socia_compra]", parameters).Tables[0];
            return ConvertTo.TableToList(table);
        }

        public static List<Dictionary<string, object>> partDataResult()
        {           
            DataTable table = Execute.StoreProcedure("[dbo].[usp_cubo_participante]").Tables[0];
            return ConvertTo.TableToList(table);
        }

        public static List<aspnetusers_participante> GetAspNetUsers(string userId)
        {
            using (DbContextJulio db = new  DbContextJulio())
            {
                SqlParameter param = new SqlParameter("@user_id", userId);
                return db.Database.SqlQuery<aspnetusers_participante>("[dbo].[usp_aspnetusers_participante] @user_id", param).ToList<aspnetusers_participante>();
            }
        }
    }

    public static class Execute
    {
        public static DataSet StoreProcedure(string storeProcedureName, Dictionary<string, object> parameter = null)
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                DataSet dataSet = dbContext.GetDataSet(storeProcedureName, CommandType.StoredProcedure, parameter);
                return dataSet;
            }
        }
    }

    public static class ConvertTo
    {
        public static List<Dictionary<string, object>> TableToList(DataTable table)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in table.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return rows;
        }
    }


    public interface ICallUnit
    {
        /// <summary>
        /// Obtener lista de status de llamada
        /// </summary>
        /// <returns></returns>
        List<cBase> GetCallStatusList();

        /// <summary>
        /// Obtener citas, ejecutivo que utiliza el módulo
        /// </summary>
        /// <returns></returns>
        List<cCitas> GetMeetingDateList();

        /// <summary>
        /// Obtener lista de télefonos (id = participante campaña)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<cTelefonos> GetPhoneNumberList(decimal id);

        /// <summary>
        /// Obtener id => participante_campaña (campaña_id)
        /// </summary>
        /// <returns></returns>
        int GetParticipantCampaignId(int camp_id);

        /// <summary>
        /// Obtener participante (id = participante campaña)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        cParticipante GetParticipant(decimal id);

        /// <summary>
        /// Guardar datos del registro seleccionado
        /// </summary>
        /// <param name="model"></param>
        void SaveLogs(Entities.Modulos.Llamada.Models.FormLog model);

        /// <summary>
        /// Guardar la fecha de la reunión del participante
        /// </summary>
        /// <param name="model"></param>
        void SaveSchedule(Entities.Modulos.Llamada.Models.FormSchedule model);

        /// <summary>
        /// Actualizar status cita 
        /// </summary>
        /// <param name="id"></param>
        void UpdateScheduleStatus(decimal id);

        /// <summary>
        /// Obtener participantes (busqueda de registros)
        /// </summary>      
        List<cBusqueda> GetParticipantList(string parameters, decimal campaign_id);

        /// <summary>
        /// Obtener lista de campañas
        /// </summary>
        /// <returns></returns>
        List<cBase> GetCampaignList();

        /// <summary>
        /// Obtener Total de Llamadas de la Campaña y las Llamadas Cerradas
        /// </summary>
        /// <returns></returns>
        cBase.cTotal_Llamadas GetCountCampaniaLlamadas(decimal campaign_id);

        /// <summary>
        /// Obtener participantes históricos (histórico)
        /// </summary>      
        List<cHistorico> GetParticipantLogList(string parameters, decimal campaign_id);

    }

    public class CallUnit : ICallUnit
    {
        public string userId { get; set; }

        public CallUnit(string _userId)
        {
            this.userId = _userId;
        }

        public List<cBase> GetCallStatusList()
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                SqlParameter parameters = new SqlParameter("@userId", this.userId);
                return dbContext.Database.SqlQuery<cBase>("[dbo].[usp_call_GetCallStatusList] @userId", parameters).ToList<cBase>();
            }
        }

        public List<cCitas> GetMeetingDateList()
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_GetMeetingDateList] @userId";
                var sqlParams = new[]{
                    new SqlParameter("userId", this.userId)
                };
                return dbContext.Database.SqlQuery<cCitas>(sqlText, sqlParams).ToList<cCitas>();
            }
        }

        public int GetParticipantCampaignId(int camp_id)
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_GetParticipantCampaignId] @user_id, @campaign_id";
                var sqlParams = new[]{
                    new SqlParameter("user_id", userId),
                    new SqlParameter("campaign_id", camp_id)
                };
                return dbContext.Database.SqlQuery<int>(sqlText, sqlParams).FirstOrDefault();
            }
        }

        public List<cTelefonos> GetPhoneNumberList(decimal id)
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_GetPhoneNumberList] @participant_campaign_id, @user_id";
                var sqlParams = new[]{
                    new SqlParameter("participant_campaign_id", id),
                    new SqlParameter("user_id", this.userId)
                };
                return dbContext.Database.SqlQuery<cTelefonos>(sqlText, sqlParams).ToList<cTelefonos>();
            }
        }

        public cParticipante GetParticipant(decimal id)
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_GetParticipant] @participant_campaign_id, @user_id";
                var sqlParams = new[]{
                    new SqlParameter("participant_campaign_id", id),
                    new SqlParameter("user_id", this.userId)
                };
                return dbContext.Database.SqlQuery<cParticipante>(sqlText, sqlParams).FirstOrDefault();
            }
        }

        public cBase.cTotal_Llamadas GetCountCampaniaLlamadas(decimal campaign_id)
        {            
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_GetCountCampaniaLlamadas] @campaign_id";
                var sqlParams = new[]{
                    new SqlParameter("campaign_id", campaign_id),
                };
                return dbContext.Database.SqlQuery<cBase.cTotal_Llamadas>(sqlText, sqlParams).FirstOrDefault();
            }
        }

        public void SaveLogs(Entities.Modulos.Llamada.Models.FormLog model)
        {
            #region Parameter SQL
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@participante_campaña_id", model.LogId);
            parameters.Add("@participante_telefono_id", model.TelefonoId);
            parameters.Add("@status_llamada_id", model.StatusId);
            parameters.Add("@comentarios", model.Comments);
            parameters.Add("@usuario_id", this.userId);
            #endregion        
            Execute.StoreProcedure("[dbo].[usp_call_SaveLogs]", parameters);
        }

        public void SaveSchedule(Entities.Modulos.Llamada.Models.FormSchedule model)
        {
            model.FullDate = Convert.ToDateTime($"{model.Date} {model.Hours}");
            #region Parameter SQL
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@participante_campaña_id", model.ScheduleId);
            parameters.Add("@participante_telefono_id", model.PhoneNumberId);
            parameters.Add("@meeting_date", model.FullDate);
            parameters.Add("@comentarios", model.Comments);
            parameters.Add("@usuario_id", this.userId);
            #endregion        
            var result = Execute.StoreProcedure("[dbo].[usp_call_SaveSchedule]", parameters);
        }

        public void UpdateScheduleStatus(decimal id)
        {
            #region Parameter SQL
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@participant_campaign_id", id);
            parameters.Add("@user_id", this.userId);
            #endregion        
            var result = Execute.StoreProcedure("[dbo].[usp_call_UpdateScheduleStatus]", parameters);
        }

        public List<cBusqueda> GetParticipantList(string parameters, decimal campaign_id)
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_SearchParticipant] @parameters, @campaign_id, @user_id";
                var sqlParams = new[]{
                    new SqlParameter("parameters", parameters),
                    new SqlParameter("campaign_id",campaign_id),
                    new SqlParameter("user_id", this.userId)
                };
                return dbContext.Database.SqlQuery<cBusqueda>(sqlText, sqlParams).ToList<cBusqueda>();
            }
        }

        public List<cBase> GetCampaignList()
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                SqlParameter parameters = new SqlParameter("@userId", this.userId);
                return dbContext.Database.SqlQuery<cBase>("[dbo].[usp_call_GetCampaignList] @userId", parameters).ToList<cBase>();
            }
        }

        public List<cHistorico> GetParticipantLogList(string parameters, decimal campaign_id)
        {
            List<cHistorico> historic = new List<cHistorico>();
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_SearchParticipantHistoric] @parameters, @campaign_id, @user_id";
                var sqlParams = new[]{
                    new SqlParameter("parameters", parameters),
                    new SqlParameter("campaign_id",campaign_id),
                    new SqlParameter("user_id", this.userId)
                };
                historic = dbContext.Database.SqlQuery<cHistorico>(sqlText, sqlParams).ToList<cHistorico>();
            }

            foreach (var item in historic)
            {
                item.detalles = GetParticipantMarker(item.id);
            }

            return historic;

        }

        private List<cDetalles> GetParticipantMarker(decimal part_campaign_id)
        {
            using (DbContextJulio dbContext = new DbContextJulio())
            {
                var sqlText = "[dbo].[usp_call_GetParticipantMarker] @participant_campaign_id, @user_id";
                var sqlParams = new[]{
                        new SqlParameter("@participant_campaign_id",part_campaign_id),
                        new SqlParameter("user_id", this.userId)
                    };
                return dbContext.Database.SqlQuery<cDetalles>(sqlText, sqlParams).ToList<cDetalles>();
            }
        }

    }
}
