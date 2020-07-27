namespace JulioLoyalty.Entities
{
	public class DatosMailing
	{
		public decimal ProgramaID
		{ get; set; }
		public string Programa
		{ get; set; }
		public string FromMail
		{ get; set; }
		public string FromName
		{ get; set; }
		public string ServidorSMTP
		{ get; set; }
		public string ServidorPOP
		{ get; set; }
		public string UsuarioMail
		{ get; set; }
		public string PasswordMail
		{ get; set; }
	}
	public class DatosClienteMailing
	{
		public string FromMail
		{ get; set; }
		public string FromName
		{ get; set; }
		public string ToMail
		{ get; set; }
		public string ToName
		{ get; set; }
		public string Subject
		{ get; set; }
	}
	public class RespuestaEnvio
	{
		public RespuestaEnvio()
		{ }
		public RespuestaEnvio(string codigo, string mensaje)
		{
			Codigo = codigo;
			Mensaje = mensaje;
		}
		public string Codigo
		{ get; set; }
		public string Mensaje
		{ get; set; }
	}
}
