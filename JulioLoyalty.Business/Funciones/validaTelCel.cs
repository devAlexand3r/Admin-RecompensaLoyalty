using JulioLoyalty.Entities.ValidaCorreo;
using JulioLoyalty.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Funciones
{
    public class validaTelCel
    {
        public bool ExisteTelCelNoPermitido(string telcel)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@telefono", telcel);
                DataSet setTables = db.GetDataSet("[dbo].[usp_Consulta_telefonos_no_permitidos]", CommandType.StoredProcedure, parameters);
                if (setTables.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public csRespuestaValidacion BuscaParticipante_TelCel(string telcel, decimal participante_id)
        {
            csRespuestaValidacion result = new csRespuestaValidacion();
            using (DbContextJulio db = new DbContextJulio())
            {
                var _participante_TelCel = (from p in db.participante
                                            join pt in db.participante_telefono on p.id equals pt.participante_id
                                            join t in db.transaccion on p.id equals t.participante_id
                                            where pt.telefono == telcel.Trim() && pt.tipo_telefono_id == 3 && p.status_participante_id != 13 && p.status_participante_id != 14 && t.tipo_transaccion_id == 1 && p.id != participante_id
                                            select new
                                            {
                                                p.id,
                                                p.clave,
                                                p.nombre,
                                                p.apellido_paterno,
                                                p.apellido_materno,
                                                p.correo_electronico,
                                                p.fecha_nacimiento,
                                                pt.telefono,
                                                t.fecha
                                            }).OrderByDescending(t => t.fecha).FirstOrDefault();
                if (_participante_TelCel != null)
                {
                    result.id = _participante_TelCel.id;
                    result.clave = _participante_TelCel.clave;
                    result.nombre = _participante_TelCel.nombre;
                    result.apellido_paterno = _participante_TelCel.apellido_paterno;
                    result.apellido_materno = _participante_TelCel.apellido_materno;
                    result.correo_electronico = null;
                    result.fecha_nacimiento = _participante_TelCel.fecha_nacimiento;
                    result.telefono_celular = _participante_TelCel.telefono;
                }
            }
            return result;
        }
    }
}