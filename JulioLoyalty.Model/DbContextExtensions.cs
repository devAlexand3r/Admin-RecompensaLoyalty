using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Model
{
	public static class DbContextExtensions
	{
		/// <summary>
		/// Devuelve un DataSet
		/// </summary>
		/// <param name="context"></param>
		/// <param name="sql">consulta sql, procedimiento almacenado o nombre de la tabla</param>
		/// <param name="commandType">tipo de comando</param>
		/// <param name="parameters">parametros a incluir en la consulta</param>
		/// <returns></returns>
		public static DataSet GetDataSet(this DbContext context, string sql, CommandType commandType, Dictionary<string, object> parameters = null)
		{

			var connectionState = context.Database.Connection.State;

			var result = new DataSet();

			var cmd = context.Database.Connection.CreateCommand();
			cmd.CommandType = commandType;
			cmd.CommandText = sql;
            cmd.CommandTimeout = 5000;
            if (parameters != null)
			{
				foreach (var pr in parameters)
				{
					var p = cmd.CreateParameter();
					p.ParameterName = pr.Key;
					p.Value = pr.Value;
					cmd.Parameters.Add(p);
				}
			}

			try
			{
				if (connectionState != ConnectionState.Open)
				{
					context.Database.Connection.Open();
             

                }

				var reader = cmd.ExecuteReader();

				do
				{
					var tb = new DataTable();
					tb.Load(reader);
					result.Tables.Add(tb);

				} while (!reader.IsClosed);
			}
			finally
			{
				if (connectionState == ConnectionState.Closed)
				{
					context.Database.Connection.Close();
				}
			}


			return result;
		}
	}
}
