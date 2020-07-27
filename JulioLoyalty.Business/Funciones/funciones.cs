using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Funciones
{
	public class funciones
	{
		public void guardalog(string cadena)
		{
			StreamWriter writer;
			DateTime Fecha = DateTime.Now;
			String NombreLog = "LogOperacion" + Fecha.ToString("ddMMyyyy") + ".txt";
			string ruta = ConfigurationManager.AppSettings.Get("RutaLog").ToString() + NombreLog;
			writer = File.AppendText(ruta);
			writer.WriteLine(Fecha.ToString("dd/MM/yyyy hh:mm:ss tt ") + cadena);
			writer.Close();
		}
	}
}
