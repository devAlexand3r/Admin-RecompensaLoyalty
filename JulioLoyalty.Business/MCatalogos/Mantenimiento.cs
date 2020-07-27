using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JulioLoyalty.Business.Parameters;

namespace JulioLoyalty.Business.MCatalogos
{
    public class Mantenimiento : IMantenimiento
    {
        private string querySQL;
        private string queryWHERE;
        private string queryValues;
        private string userId;

        private ResultJson result = new ResultJson();
        public Mantenimiento() { }

        /// <summary>
        /// Actualizar registro de manera dinámica al catalogo indicado a travez de una consulta SQL parametrizada
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public ResultJson ActualizarMantenimiento(SQLParameters sqlParameters)
        {
            try
            {
                using (Model.DbContextJulio db = new Model.DbContextJulio())
                {
                    userId = db.AspNetUsers.Where(s => s.UserName == sqlParameters.userName).FirstOrDefault().Id;                    
                    querySQL = $"UPDATE {sqlParameters.tableName} SET";

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    foreach (var columna in sqlParameters.Columns)
                    {
                        parameters.Add(new SqlParameter($"@{columna.Name}", darFormato(columna)));
                        if (columna.Name == "id")
                            queryWHERE = $" WHERE {columna.Name} =  @{columna.Name}";
                        else
                            querySQL += $" {columna.Name} = @{columna.Name},";
                    }
                    querySQL = $"{querySQL.TrimEnd(',')} {queryWHERE} ";
                    //Ejecutamos la consulta sql y con los parametros que necesita, si todo es correcto nos devolvera el nuemero de registros que fue actualizado
                    int noOfRowInserted = db.Database.ExecuteSqlCommand(querySQL, parameters.ToArray());
                }
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

        /// <summary>
        /// Agregar registro de manera dinámica al catalogo indicado a travez de una consulta SQL parametrizada
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public ResultJson AgregarMantenimiento(SQLParameters sqlParameters)
        {
            try
            {
                using (Model.DbContextJulio ctx = new Model.DbContextJulio())
                {
                    userId = ctx.AspNetUsers.Where(s => s.UserName == sqlParameters.userName).FirstOrDefault().Id;
                    querySQL = $"INSERT INTO {sqlParameters.tableName} (";

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    foreach (var columna in sqlParameters.Columns)
                    {
                        parameters.Add(new SqlParameter($"@{columna.Name}", darFormato(columna)));
                        querySQL += $"{columna.Name},";
                        queryValues += $"@{columna.Name},";

                    }
                    querySQL = $"{querySQL.TrimEnd(',')}) Values({queryValues.TrimEnd(',')})";
                    //Ejecutamos la consulta SQL con los parametros necesarios, si todo es correcto nos devolvera el numero de registros que fueron registrados
                    int noOfRowInserted = ctx.Database.ExecuteSqlCommand(querySQL, parameters.ToArray());
                }
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

        /// <summary>
        /// Eliminar registro de manera dinámica al catalogo indicado a travez de una consulta sQL parametrizada
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public ResultJson EliminarCEMantenimiento(SQLParameters sqlParameters)
        {
            try
            {
                using (Model.DbContextJulio db = new Model.DbContextJulio())
                {
                    userId = db.AspNetUsers.Where(s => s.UserName == sqlParameters.userName).FirstOrDefault().Id;
                    querySQL = $"DELETE {sqlParameters.tableName} WHERE ";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    foreach (var columna in sqlParameters.Columns)
                    {
                        parameters.Add(new SqlParameter($"@{columna.Name}", darFormato(columna)));
                        querySQL += $"{columna.Name} = @{columna.Name}";
                    }
                    //Ejecutamos la consulta sql y con los parametros que necesita, si todo es correcto nos devolvera el nuemero de registros que fue actualizado
                    int noOfRowInserted = db.Database.ExecuteSqlCommand(querySQL, parameters.ToArray());
                }
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

        /// <summary>
        /// Conversion de datos
        /// </summary>
        /// <param name="columna"></param>
        /// <returns></returns>
        private object darFormato(Column columna)
        {
            if (columna.type == "varchar" || columna.type == "nvarchar")
                return columna.value;

            if (columna.type == "datetime")
                return DateTime.Now;

            if (columna.type == "uniqueidentifier")
                return userId;

            if (columna.type == "decimal")
                return decimal.Parse(columna.value);

            if (columna.type == "int")
                return int.Parse(columna.value);

            if (columna.type == "bit")
                return bool.Parse(columna.value);

            if (String.IsNullOrEmpty(columna.value))
                return 1;


            return columna.value;
        }

    }
}
