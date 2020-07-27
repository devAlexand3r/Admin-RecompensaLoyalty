using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Model.EntitiesModels;
using JulioLoyalty.Model;
using System.Data;

namespace JulioLoyalty.Business.Transacciones
{
    public class Transaccion : IRepositorioTransaccion
    {
        private ResultJson result = new ResultJson();
        public Transaccion() { }

        public ResultJson AgregarTransaccion(RequestTransaction transaction)
        {
            try
            {
                using (Model.DbContextJulio db = new Model.DbContextJulio())
                {
                    var _usuarioLogeado = db.AspNetUsers.Where(s => s.UserName == transaction.userName).FirstOrDefault();
                   
                    Model.EntitiesModels.transaccion _transaccion = new Model.EntitiesModels.transaccion()
                    {
                        participante_id = transaction.Participante_id,
                        tipo_transaccion_id = transaction.Tipo_transaccion_id,
                        fecha = DateTime.Parse(transaction.Fecha_transaccion),
                        puntos = transaction.Puntos,
                        importe = 0,
                        puntos_redimidos = 0,
                        usuario_id = Guid.Parse(_usuarioLogeado.Id),
                        fecha_captura = DateTime.Now
                    };
                    db.transaccion.Add(_transaccion);
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(transaction.comentarios))
                    {
                        transaccion_comentarios tc = new transaccion_comentarios()
                        {
                            transaccion_id = _transaccion.id,
                            comentarios = transaction.comentarios,
                            fecha_alta = DateTime.Now,
                            usuario_alta_id = Guid.Parse(_usuarioLogeado.Id)
                        };
                        db.transaccion_comentarios.Add(tc);
                        db.SaveChanges();
                    }

					// Actualiza Saldo Participante
					Dictionary<string, object> parameters = new Dictionary<string, object>();
					parameters.Add("@participante_id", transaction.Participante_id);
					DataSet setTables = db.GetDataSet("[dbo].[sp_actualiza_saldo_participante_todo]", CommandType.StoredProcedure, parameters);
					//
					result.Message = "La transacción se agrego correctamente";
                };
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error en la transacción, vuelva intentarlo mas tarde";
                result.InnerException = $"{ex.Message}";
                return result;
            }
        }
    }
}
