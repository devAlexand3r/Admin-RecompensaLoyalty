using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Entities.ValidaCorreo;
using JulioLoyalty.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using JulioLoyalty.Model.EntitiesModels;
using System.Data;

namespace JulioLoyalty.Business.Funciones
{
    public class ValidaCorreo
    {
        private string urlValidaCorreo = ConfigurationManager.AppSettings["urlValidaCorreo"].ToString();
        public csValidaCorreo.ValidaCorreo Valida(string email)
        {
            csValidaCorreo.ValidaCorreo valida = new csValidaCorreo.ValidaCorreo();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlValidaCorreo + "/" + email);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    valida = JsonConvert.DeserializeObject<csValidaCorreo.ValidaCorreo>(json);
                }
            }
            catch (Exception ex)
            {
                valida.ErrorException = ex.Message;
            }
            return valida;
        }

        public bool ExisteEmailHistorico(string email)
        {
            using (Model.DbContextJulio entities = new DbContextJulio())
            {
                var result = entities.historico_emails.Where(s => s.correo_electronico == email).FirstOrDefault();
                if (result == null) // No esta el correo				
                    return false;
                else
                    return true; // Si esta el correo
            }
        }

        public bool InsertaHistoricoEmail(decimal participante_id, string email, Guid usuario_id, string status)
        {
            try
            {
                using (Model.DbContextJulio entities = new DbContextJulio())
                {
                    var insert = new historico_emails()
                    {
                        participante_id = participante_id,
                        correo_electronico = email,
                        fecha_alta = DateTime.Now,
                        usuario_alta_id = usuario_id,
                        status = status
                    };
                    entities.historico_emails.Add(insert);
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string ObtieneStatusEmail(string email)
        {
            try
            {
                using (Model.DbContextJulio entities = new DbContextJulio())
                {
                    var result = entities.historico_emails.Where(s => s.correo_electronico == email).FirstOrDefault();
                    if (result == null) // No esta el correo				
                        return string.Empty;
                    else
                        return result.status; // Si esta el correo y obtiene el status
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public csRespuestaValidacion BuscaParticipante_Email(string email, decimal participante_id)
        {
            csRespuestaValidacion result = new csRespuestaValidacion();
            using (DbContextJulio db = new DbContextJulio())
            {
                var _participante = (from p in db.participante
                                     join t in db.transaccion on p.id equals t.participante_id
                                     where p.correo_electronico == email.Trim() && p.status_participante_id != 13 && p.status_participante_id != 14 && t.tipo_transaccion_id == 1 && p.id != participante_id
                                     select new
                                     {
                                         id = p.id,
                                         clave = p.clave,
                                         nombre = p.nombre,
                                         apellido_paterno = p.apellido_paterno,
                                         apellido_materno = p.apellido_materno,
                                         correo_electronico = p.correo_electronico,
                                         fecha_nacimiento = p.fecha_nacimiento,
                                         fecha = t.fecha
                                     }).OrderByDescending(t => t.fecha).FirstOrDefault();
                if (_participante != null)
                {
                    result.id = _participante.id;
                    result.clave = _participante.clave;
                    result.nombre = _participante.nombre;
                    result.apellido_paterno = _participante.apellido_paterno;
                    result.apellido_materno = _participante.apellido_materno;
                    result.correo_electronico = _participante.correo_electronico;
                    result.fecha_nacimiento = _participante.fecha_nacimiento;
                    result.telefono_celular = null;
                }
            }
            return result;
        }

        public csRespuestaValidacion ConsultaDominio_Email(string email)
        {
            csRespuestaValidacion result = new csRespuestaValidacion();
            try
            {
                using (DbContextJulio db = new DbContextJulio())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@email", email);
                    DataSet setTables = db.GetDataSet("[dbo].[usp_Consulta_dominio_email]", CommandType.StoredProcedure, parameters);
                    if (setTables.Tables[0].Rows.Count > 1)
                    {
                        result.status = Int16.Parse(setTables.Tables[0].Rows[0]["status"].ToString());
                        result.result = setTables.Tables[0].Rows[0]["result"].ToString();
                        result.message = setTables.Tables[0].Rows[0]["message"].ToString();
                        return result;
                    }
                    else
                    {
                        result.status = 0;
                        result.result = "ok";
                        result.message = "Correo valido";
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}