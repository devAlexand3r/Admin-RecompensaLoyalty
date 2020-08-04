namespace JulioLoyalty.Model
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using EntitiesModels;
	using System.Data.Entity.Validation;

	public partial class DbContextJulio : DbContext
	{
		public DbContextJulio()
			: base("name=ConnectionLoyalty")
		{
		}

		public override int SaveChanges()
		{
			try
			{
				return base.SaveChanges();
			}
			catch (DbEntityValidationException ex)
			{
				ExceptionLogging.SendErrorToText(ex);
				throw;
			}
		}

		public virtual DbSet<AccessByUser> AccessByUser { get; set; }
		public virtual DbSet<agenda_llamadas> agenda_llamadas { get; set; }
		public virtual DbSet<agente> agente { get; set; }
		public virtual DbSet<almacen_tarjetas> almacen_tarjetas { get; set; }
		public virtual DbSet<AspNetProfiles> AspNetProfiles { get; set; }
		public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
		public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
		public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
		public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
		public virtual DbSet<AspNetUsers_Distribuidor> AspNetUsers_Distribuidor { get; set; }
		public virtual DbSet<AspNetUsers_Participante> AspNetUsers_Participante { get; set; }
		public virtual DbSet<base_nse> base_nse { get; set; }
		public virtual DbSet<bitacora_accesos> bitacora_accesos { get; set; }
		public virtual DbSet<bitacora_activaciones> bitacora_activaciones { get; set; }
		public virtual DbSet<bitacora_transferencias> bitacora_transferencias { get; set; }
		public virtual DbSet<caja> caja { get; set; }
		public virtual DbSet<calendario_canje> calendario_canje { get; set; }
		public virtual DbSet<CatalogoErroresWS> CatalogoErroresWS { get; set; }
		public virtual DbSet<catalogos_lealtad> catalogos_lealtad { get; set; }
		public virtual DbSet<catalogos_relaciones> catalogos_relaciones { get; set; }
		public virtual DbSet<categoria_premio> categoria_premio { get; set; }
		public virtual DbSet<categoria_producto> categoria_producto { get; set; }
		public virtual DbSet<categoria_tipo_llamada> categoria_tipo_llamada { get; set; }
		public virtual DbSet<categoria_transaccion> categoria_transaccion { get; set; }
		public virtual DbSet<clave_rms> clave_rms { get; set; }
		public virtual DbSet<color> color { get; set; }
		public virtual DbSet<credito_debito> credito_debito { get; set; }
		public virtual DbSet<distribuidor> distribuidor { get; set; }
		public virtual DbSet<distribuidor_caja> distribuidor_caja { get; set; }
		public virtual DbSet<distribuidor_direccion> distribuidor_direccion { get; set; }
		public virtual DbSet<encuesta> encuesta { get; set; }
		public virtual DbSet<error_envio_RMS> error_envio_RMS { get; set; }
		public virtual DbSet<error_recarga_sin_saldo_suficiente> error_recarga_sin_saldo_suficiente { get; set; }
		public virtual DbSet<estado_civil> estado_civil { get; set; }
		public virtual DbSet<familia> familia { get; set; }
		public virtual DbSet<forma_pago> forma_pago { get; set; }
		public virtual DbSet<historial_status_premio> historial_status_premio { get; set; }
		public virtual DbSet<historico_acumulacion> historico_acumulacion { get; set; }
		public virtual DbSet<historico_emails> historico_emails { get; set; }
		public virtual DbSet<historico_log> historico_log { get; set; }
		public virtual DbSet<historico_tarjeta_session_WS> historico_tarjeta_session_WS { get; set; }
		public virtual DbSet<historico_tarjetas> historico_tarjetas { get; set; }
		public virtual DbSet<historico_ventas> historico_ventas { get; set; }
		public virtual DbSet<historico_ventas_forma_pago> historico_ventas_forma_pago { get; set; }
		public virtual DbSet<historico_ventas_producto> historico_ventas_producto { get; set; }
		public virtual DbSet<linea> linea { get; set; }
		public virtual DbSet<llamada> llamada { get; set; }
		public virtual DbSet<llamada_seguimiento> llamada_seguimiento { get; set; }
		public virtual DbSet<llamada_tipo_llamada> llamada_tipo_llamada { get; set; }
		public virtual DbSet<mailchimp_campaignSettings> mailchimp_campaignSettings { get; set; }
		public virtual DbSet<mailchimp_list> mailchimp_list { get; set; }
		public virtual DbSet<mailing> mailing { get; set; }
		public virtual DbSet<marca_premio> marca_premio { get; set; }
		public virtual DbSet<Menu> Menu { get; set; }
		public virtual DbSet<no_participan> no_participan { get; set; }
		public virtual DbSet<ocupacion> ocupacion { get; set; }
		public virtual DbSet<operadora> operadora { get; set; }
		public virtual DbSet<pais> pais { get; set; }
		public virtual DbSet<participante> participante { get; set; }
		public virtual DbSet<participante_direccion> participante_direccion { get; set; }
		public virtual DbSet<participante_nivel> participante_nivel { get; set; }
		public virtual DbSet<participante_nivel_historico> participante_nivel_historico { get; set; }
		public virtual DbSet<participante_status_comentarios> participante_status_comentarios { get; set; }
		public virtual DbSet<participante_tarjeta> participante_tarjeta { get; set; }
		public virtual DbSet<participante_telefono> participante_telefono { get; set; }
		public virtual DbSet<premio> premio { get; set; }
		public virtual DbSet<producto> producto { get; set; }
		public virtual DbSet<proveedor_premios> proveedor_premios { get; set; }
		public virtual DbSet<rama> rama { get; set; }
		public virtual DbSet<saldos_participante> saldos_participante { get; set; }
		public virtual DbSet<sexo> sexo { get; set; }
		public virtual DbSet<tema> tema { get; set; }
		public virtual DbSet<status> status { get; set; }
		public virtual DbSet<status_carga> status_carga { get; set; }
		public virtual DbSet<status_llamada> status_llamada { get; set; }
		public virtual DbSet<status_participante> status_participante { get; set; }
		public virtual DbSet<status_premio> status_premio { get; set; }
		public virtual DbSet<status_seguimiento> status_seguimiento { get; set; }
		public virtual DbSet<status_tarjeta> status_tarjeta { get; set; }
		public virtual DbSet<status_transaccion> status_transaccion { get; set; }
		public virtual DbSet<SubMenu> SubMenu { get; set; }
		public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
		public virtual DbSet<talla> talla { get; set; }
		public virtual DbSet<temporada> temporada { get; set; }
		public virtual DbSet<tipo_agente> tipo_agente { get; set; }
		public virtual DbSet<tipo_direccion> tipo_direccion { get; set; }
		public virtual DbSet<tipo_llamada> tipo_llamada { get; set; }
		public virtual DbSet<tipo_pago> tipo_pago { get; set; }
		public virtual DbSet<tipo_participante> tipo_participante { get; set; }
		public virtual DbSet<tipo_tarjeta> tipo_tarjeta { get; set; }
		public virtual DbSet<tipo_telefono> tipo_telefono { get; set; }
		public virtual DbSet<tipo_transaccion> tipo_transaccion { get; set; }
		public virtual DbSet<tmp_agentes> tmp_agentes { get; set; }
		public virtual DbSet<tmp_cajas> tmp_cajas { get; set; }
		public virtual DbSet<tmp_catproductos> tmp_catproductos { get; set; }
		public virtual DbSet<tmp_formaspagos> tmp_formaspagos { get; set; }
		public virtual DbSet<tmp_sucursales> tmp_sucursales { get; set; }
		public virtual DbSet<tmp_tickets> tmp_tickets { get; set; }
		public virtual DbSet<tmp_tickets_formasdepago> tmp_tickets_formasdepago { get; set; }
		public virtual DbSet<tmp_tickets_productos> tmp_tickets_productos { get; set; }
		public virtual DbSet<transaccion> transaccion { get; set; }
		public virtual DbSet<transaccion_comentarios> transaccion_comentarios { get; set; }
		public virtual DbSet<transaccion_detalle> transaccion_detalle { get; set; }
		public virtual DbSet<transaccion_premio> transaccion_premio { get; set; }
		public virtual DbSet<vendedor> vendedor { get; set; }
		public virtual DbSet<bitacora_envios> bitacora_envios { get; set; }
		public virtual DbSet<nivel> nivel { get; set; }
		public virtual DbSet<sepomex> sepomex { get; set; }
		public virtual DbSet<transaccion_redencion> transaccion_redencion { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<agenda_llamadas>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<agenda_llamadas>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<agenda_llamadas>()
				.Property(e => e.tipo_llamada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<agenda_llamadas>()
				.Property(e => e.titulo)
				.IsUnicode(false);

			modelBuilder.Entity<agenda_llamadas>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<agenda_llamadas>()
				.Property(e => e.status_llamada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<agente>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<agente>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<agente>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<agente>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<agente>()
				.Property(e => e.tipo_agente_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<agente>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.tarjeta)
				.IsUnicode(false);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.codigo)
				.IsUnicode(false);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.addon)
				.IsUnicode(false);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.codbar)
				.IsUnicode(false);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.digito_control)
				.IsUnicode(false);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.tipo_tarjeta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.status_tarjeta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<almacen_tarjetas>()
				.Property(e => e.distribuidor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<AspNetRoles>()
				.HasMany(e => e.AspNetUsers)
				.WithMany(e => e.AspNetRoles)
				.Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

			modelBuilder.Entity<AspNetUsers>()
				.HasOptional(e => e.AspNetProfiles)
				.WithRequired(e => e.AspNetUsers)
				.WillCascadeOnDelete();

			modelBuilder.Entity<AspNetUsers>()
				.HasMany(e => e.AspNetUserClaims)
				.WithRequired(e => e.AspNetUsers)
				.HasForeignKey(e => e.UserId);

			modelBuilder.Entity<AspNetUsers>()
				.HasMany(e => e.AspNetUserLogins)
				.WithRequired(e => e.AspNetUsers)
				.HasForeignKey(e => e.UserId);

			modelBuilder.Entity<AspNetUsers>()
				.HasMany(e => e.AspNetUsers_Participante)
				.WithRequired(e => e.AspNetUsers)
				.HasForeignKey(e => e.UserId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<AspNetUsers>()
				.HasMany(e => e.AspNetUsers_Distribuidor)
				.WithRequired(e => e.AspNetUsers)
				.HasForeignKey(e => e.IdUser)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<AspNetUsers_Distribuidor>()
				.Property(e => e.IdDistribuidor)
				.HasPrecision(18, 0);

			modelBuilder.Entity<AspNetUsers_Participante>()
				.Property(e => e.ParticipanteId)
				.HasPrecision(18, 0);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.entidad_clave)
				.IsUnicode(false);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.entidad_descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.municipio_clave)
				.IsUnicode(false);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.municipio_descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.asentamiento_tipo)
				.IsUnicode(false);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.asentamiento_descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.nse)
				.IsUnicode(false);

			modelBuilder.Entity<base_nse>()
				.Property(e => e.codigo_postal)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_accesos>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<bitacora_accesos>()
				.Property(e => e.opcion)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_accesos>()
				.Property(e => e.evento)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_activaciones>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<bitacora_activaciones>()
				.Property(e => e.actividad)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_activaciones>()
				.Property(e => e.tarjeta)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_activaciones>()
				.Property(e => e.distribuidor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<bitacora_activaciones>()
				.Property(e => e.numero_ticket)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_transferencias>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<bitacora_transferencias>()
				.Property(e => e.actividad)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_transferencias>()
				.Property(e => e.tarjeta_origen)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_transferencias>()
				.Property(e => e.tarjeta_destino)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_transferencias>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<caja>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<caja>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<caja>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<caja>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<caja>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<calendario_canje>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<calendario_canje>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<CatalogoErroresWS>()
				.Property(e => e.ErrorId)
				.HasPrecision(18, 0);

			modelBuilder.Entity<CatalogoErroresWS>()
				.Property(e => e.Descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<CatalogoErroresWS>()
				.Property(e => e.Detalle)
				.IsUnicode(false);

			modelBuilder.Entity<catalogos_lealtad>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<catalogos_lealtad>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<catalogos_lealtad>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<catalogos_lealtad>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<catalogos_lealtad>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<catalogos_relaciones>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<catalogos_relaciones>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<catalogos_relaciones>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<catalogos_relaciones>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_premio>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_premio>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_premio>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_premio>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_premio>()
				.Property(e => e.url_imagen)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_premio>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_premio>()
				.Property(e => e.datafilter)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_premio>()
				.HasMany(e => e.premio)
				.WithRequired(e => e.categoria_premio)
				.HasForeignKey(e => e.categoria_premio_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<categoria_producto>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_producto>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_producto>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_producto>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_producto>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_producto>()
				.HasMany(e => e.producto)
				.WithRequired(e => e.categoria_producto)
				.HasForeignKey(e => e.categoria_producto_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<categoria_tipo_llamada>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_tipo_llamada>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_tipo_llamada>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_tipo_llamada>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_tipo_llamada>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_tipo_llamada>()
				.HasMany(e => e.tipo_llamada)
				.WithOptional(e => e.categoria_tipo_llamada)
				.HasForeignKey(e => e.categoria_tipo_llamada_id);

			modelBuilder.Entity<categoria_transaccion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_transaccion>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_transaccion>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_transaccion>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<categoria_transaccion>()
				.Property(e => e.credito_debito_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_transaccion>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<categoria_transaccion>()
				.HasMany(e => e.tipo_transaccion)
				.WithOptional(e => e.categoria_transaccion)
				.HasForeignKey(e => e.categoria_transaccion_id);

			modelBuilder.Entity<clave_rms>()
				.Property(e => e.anio)
				.IsUnicode(false);

			modelBuilder.Entity<clave_rms>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<clave_rms>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<color>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<color>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<color>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<color>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<color>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<credito_debito>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<credito_debito>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<credito_debito>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<credito_debito>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<credito_debito>()
				.HasMany(e => e.categoria_transaccion)
				.WithRequired(e => e.credito_debito)
				.HasForeignKey(e => e.credito_debito_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.url_imagen)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.prefijo_rms)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor>()
				.Property(e => e.texto)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor>()
				.HasMany(e => e.almacen_tarjetas)
				.WithOptional(e => e.distribuidor)
				.HasForeignKey(e => e.distribuidor_id);

			modelBuilder.Entity<distribuidor>()
				.HasMany(e => e.AspNetUsers_Distribuidor)
				.WithRequired(e => e.distribuidor)
				.HasForeignKey(e => e.IdDistribuidor)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<distribuidor>()
				.HasMany(e => e.distribuidor_caja)
				.WithRequired(e => e.distribuidor)
				.HasForeignKey(e => e.distribuidor_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<distribuidor>()
				.HasMany(e => e.distribuidor_direccion)
				.WithRequired(e => e.distribuidor)
				.HasForeignKey(e => e.distribuidor_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<distribuidor>()
				.HasMany(e => e.participante)
				.WithOptional(e => e.distribuidor)
				.HasForeignKey(e => e.distribuidor_id);

			modelBuilder.Entity<distribuidor>()
				.HasMany(e => e.participante_tarjeta)
				.WithOptional(e => e.distribuidor)
				.HasForeignKey(e => e.distribuidor_id);

			modelBuilder.Entity<distribuidor_caja>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<distribuidor_caja>()
				.Property(e => e.distribuidor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<distribuidor_caja>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor_caja>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor_caja>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor_direccion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<distribuidor_direccion>()
				.Property(e => e.distribuidor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<distribuidor_direccion>()
				.Property(e => e.razon_social)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor_direccion>()
				.Property(e => e.entre_calle)
				.IsUnicode(false);

			modelBuilder.Entity<distribuidor_direccion>()
				.Property(e => e.y_calle)
				.IsUnicode(false);

			modelBuilder.Entity<encuesta>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<encuesta>()
				.Property(e => e.tipo_encuesta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<encuesta>()
				.Property(e => e.nombre)
				.IsUnicode(false);

			modelBuilder.Entity<encuesta>()
				.Property(e => e.status_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<encuesta>()
				.Property(e => e.guion)
				.IsUnicode(false);

			modelBuilder.Entity<encuesta>()
				.Property(e => e.cierre)
				.IsUnicode(false);

			modelBuilder.Entity<error_envio_RMS>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_envio_RMS>()
				.Property(e => e.transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_envio_RMS>()
				.Property(e => e.pedido_rms)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_envio_RMS>()
				.Property(e => e.tipo_direccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_envio_RMS>()
				.Property(e => e.country_code)
				.IsUnicode(false);

			modelBuilder.Entity<error_envio_RMS>()
				.Property(e => e.mensaje)
				.IsUnicode(false);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.transaccion_premio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.premio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.celular)
				.IsUnicode(false);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.monto)
				.IsUnicode(false);

			modelBuilder.Entity<error_recarga_sin_saldo_suficiente>()
				.Property(e => e.compania)
				.IsUnicode(false);

			modelBuilder.Entity<estado_civil>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<estado_civil>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<estado_civil>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<estado_civil>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<estado_civil>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<familia>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<familia>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<familia>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<familia>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<familia>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<forma_pago>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<forma_pago>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<forma_pago>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<forma_pago>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<forma_pago>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historial_status_premio>()
				.Property(e => e.transaccion_premio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historial_status_premio>()
				.Property(e => e.status_premio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historial_status_premio>()
				.Property(e => e.observaciones)
				.IsUnicode(false);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.historico_ventas_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.producto_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.cantidad)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.tipo_pago_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.nivel_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_acumulacion>()
				.Property(e => e.historico_ventas_producto_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_emails>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_emails>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_emails>()
				.Property(e => e.correo_electronico)
				.IsUnicode(false);

			modelBuilder.Entity<historico_log>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_log>()
				.Property(e => e.metodo)
				.IsUnicode(false);

			modelBuilder.Entity<historico_log>()
				.Property(e => e.parametro_principal)
				.IsUnicode(false);

			modelBuilder.Entity<historico_log>()
				.Property(e => e.parametro_producto)
				.IsUnicode(false);

			modelBuilder.Entity<historico_log>()
				.Property(e => e.parametro_formapago)
				.IsUnicode(false);

			modelBuilder.Entity<historico_log>()
				.Property(e => e.respuesta)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjeta_session_WS>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_tarjeta_session_WS>()
				.Property(e => e.tarjeta)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjeta_session_WS>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.tarjeta)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.codigo)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.addon)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.codbar)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.digito_control)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.tipo_tarjeta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.status_tarjeta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.acciones)
				.IsUnicode(false);

			modelBuilder.Entity<historico_tarjetas>()
				.Property(e => e.justificacion)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.tarjeta)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.tiendaid)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.Posid)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.transaccionid)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.Numeroticket)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.Key)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.estatusVenta)
				.IsFixedLength()
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.CodigoPromo)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.aplicabono)
				.IsFixedLength()
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.numero_autorizacion)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.Property(e => e.folio_sbx)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas>()
				.HasMany(e => e.historico_acumulacion)
				.WithRequired(e => e.historico_ventas)
				.HasForeignKey(e => e.historico_ventas_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<historico_ventas>()
				.HasMany(e => e.historico_ventas_forma_pago)
				.WithRequired(e => e.historico_ventas)
				.HasForeignKey(e => e.historico_ventas_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<historico_ventas>()
				.HasMany(e => e.historico_ventas_producto)
				.WithRequired(e => e.historico_ventas)
				.HasForeignKey(e => e.historico_ventas_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<historico_ventas_forma_pago>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_ventas_forma_pago>()
				.Property(e => e.historico_ventas_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_ventas_forma_pago>()
				.Property(e => e.forma_pago)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.clave_ean)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.Producto)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.Cantidad)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.vendedor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.historico_ventas_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.estatusVenta)
				.IsUnicode(false);

			modelBuilder.Entity<historico_ventas_producto>()
				.Property(e => e.Cantidad_Devuelta)
				.HasPrecision(18, 0);

			modelBuilder.Entity<linea>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<linea>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<linea>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<linea>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<linea>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada>()
				.Property(e => e.nombre_llama)
				.IsUnicode(false);

			modelBuilder.Entity<llamada>()
				.Property(e => e.telefono)
				.IsUnicode(false);

			modelBuilder.Entity<llamada>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<llamada>()
				.Property(e => e.agenda_llamadas_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada>()
				.Property(e => e.correo_electronico)
				.IsUnicode(false);

			modelBuilder.Entity<llamada>()
				.HasMany(e => e.llamada_seguimiento)
				.WithRequired(e => e.llamada)
				.HasForeignKey(e => e.llamada_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<llamada>()
				.HasMany(e => e.llamada_tipo_llamada)
				.WithRequired(e => e.llamada)
				.HasForeignKey(e => e.llamada_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<llamada_seguimiento>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada_seguimiento>()
				.Property(e => e.llamada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada_seguimiento>()
				.Property(e => e.observacion)
				.IsUnicode(false);

			modelBuilder.Entity<llamada_seguimiento>()
				.Property(e => e.tipo_llamada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada_seguimiento>()
				.Property(e => e.status_seguimiento_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada_tipo_llamada>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada_tipo_llamada>()
				.Property(e => e.llamada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<llamada_tipo_llamada>()
				.Property(e => e.tipo_llamada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<mailchimp_campaignSettings>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<mailchimp_campaignSettings>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_campaignSettings>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_campaignSettings>()
				.Property(e => e.ReplyTo)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_campaignSettings>()
				.Property(e => e.FromName)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_campaignSettings>()
				.Property(e => e.Title)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_campaignSettings>()
				.Property(e => e.SubjectLine)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_list>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<mailchimp_list>()
				.Property(e => e.listid)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_list>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<mailchimp_list>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<mailing>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<mailing>()
				.Property(e => e.FromMail)
				.IsUnicode(false);

			modelBuilder.Entity<mailing>()
				.Property(e => e.FromName)
				.IsUnicode(false);

			modelBuilder.Entity<mailing>()
				.Property(e => e.ToMail)
				.IsUnicode(false);

			modelBuilder.Entity<mailing>()
				.Property(e => e.ToName)
				.IsUnicode(false);

			modelBuilder.Entity<mailing>()
				.Property(e => e.Subject)
				.IsUnicode(false);

			modelBuilder.Entity<mailing>()
				.Property(e => e.pais)
				.IsUnicode(false);

			modelBuilder.Entity<marca_premio>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<marca_premio>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<marca_premio>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<marca_premio>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<marca_premio>()
				.HasMany(e => e.premio)
				.WithRequired(e => e.marca_premio)
				.HasForeignKey(e => e.marca_premio_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Menu>()
				.HasMany(e => e.SubMenu)
				.WithRequired(e => e.Menu)
				.HasForeignKey(e => e.IdMenu)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<no_participan>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<no_participan>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<ocupacion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<ocupacion>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<ocupacion>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<ocupacion>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<ocupacion>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<ocupacion>()
				.HasMany(e => e.participante)
				.WithOptional(e => e.ocupacion)
				.HasForeignKey(e => e.ocupacion_id);

			modelBuilder.Entity<operadora>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<operadora>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<operadora>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<operadora>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<operadora>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<operadora>()
				.Property(e => e.codigo_recarga)
				.IsUnicode(false);

			modelBuilder.Entity<operadora>()
				.HasMany(e => e.participante_telefono)
				.WithOptional(e => e.operadora)
				.HasForeignKey(e => e.operadora_id);

			modelBuilder.Entity<operadora>()
				.HasMany(e => e.premio)
				.WithOptional(e => e.operadora)
				.HasForeignKey(e => e.operadora_id);

			modelBuilder.Entity<pais>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<pais>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.correo_electronico)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.remitente)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.servidor_pop)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.servidor_smtp)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.usuario_correo)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.password_correo)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.prefijo_rms)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.usuario_rms)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.password_rms)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.url_programa)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.clave_carga)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.url_logo)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.plantilla_aviso)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.plantilla_terminos)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.plantilla_faqs)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.plantilla_entrega_premios)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.Property(e => e.banner_carousel)
				.IsUnicode(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.agente)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.caja)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.categoria_premio)
				.WithOptional(e => e.pais)
				.HasForeignKey(e => e.pais_id);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.categoria_producto)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.categoria_tipo_llamada)
				.WithOptional(e => e.pais)
				.HasForeignKey(e => e.pais_id);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.categoria_transaccion)
				.WithOptional(e => e.pais)
				.HasForeignKey(e => e.pais_id);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.clave_rms)
				.WithOptional(e => e.pais)
				.HasForeignKey(e => e.pais_id);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.color)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.distribuidor)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.estado_civil)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.familia)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.forma_pago)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.linea)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.ocupacion)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.operadora)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.proveedor_premios)
				.WithOptional(e => e.pais)
				.HasForeignKey(e => e.pais_id);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.rama)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.sexo)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.tema)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.status_carga)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.status_llamada)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.status_tarjeta)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.status_transaccion)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.talla)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.temporada)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.tipo_agente)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.tipo_pago)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.tipo_tarjeta)
				.WithOptional(e => e.pais)
				.HasForeignKey(e => e.pais_id);

			modelBuilder.Entity<pais>()
				.HasMany(e => e.vendedor)
				.WithRequired(e => e.pais)
				.HasForeignKey(e => e.pais_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<participante>()
				.Property(e => e.nombre)
				.IsUnicode(false);

			modelBuilder.Entity<participante>()
				.Property(e => e.apellido_paterno)
				.IsUnicode(false);

			modelBuilder.Entity<participante>()
				.Property(e => e.apellido_materno)
				.IsUnicode(false);

			modelBuilder.Entity<participante>()
				.Property(e => e.documento_identidad)
				.IsUnicode(false);

			modelBuilder.Entity<participante>()
				.Property(e => e.correo_electronico)
				.IsUnicode(false);

			modelBuilder.Entity<participante>()
				.Property(e => e.sexo_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante>()
				.Property(e => e.estado_civil_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante>()
				.Property(e => e.distribuidor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante>()
				.Property(e => e.tipo_participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante>()
				.Property(e => e.status_participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante>()
				.Property(e => e.ocupacion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.agenda_llamadas)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.AspNetUsers_Participante)
				.WithRequired(e => e.participante)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.error_recarga_sin_saldo_suficiente)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.historico_tarjetas)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.llamada)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.no_participan)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.participante_direccion)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.participante_telefono)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.participante_status_comentarios)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.saldos_participante)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante>()
				.HasMany(e => e.transaccion)
				.WithRequired(e => e.participante)
				.HasForeignKey(e => e.participante_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.tipo_direccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.calle)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.numero_interior)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.numero_exterior)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.entrecalle_1)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.entrecalle_2)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.colonia)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.codigo_postal)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.referencias)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.status_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.estado)
				.IsUnicode(false);

			modelBuilder.Entity<participante_direccion>()
				.Property(e => e.municipio)
				.IsUnicode(false);

			modelBuilder.Entity<participante_nivel>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_nivel>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_nivel>()
				.Property(e => e.nivel_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_nivel_historico>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_nivel_historico>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_nivel_historico>()
				.Property(e => e.nivel_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_status_comentarios>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_status_comentarios>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.tarjeta)
				.IsUnicode(false);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.codigo)
				.IsUnicode(false);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.addon)
				.IsUnicode(false);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.codbar)
				.IsUnicode(false);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.digito_control)
				.IsUnicode(false);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.status_tarjeta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.tipo_tarjeta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.justificacion)
				.IsUnicode(false);

			modelBuilder.Entity<participante_tarjeta>()
				.Property(e => e.distribuidor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_telefono>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_telefono>()
				.Property(e => e.tipo_telefono_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_telefono>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<participante_telefono>()
				.Property(e => e.lada)
				.IsUnicode(false);

			modelBuilder.Entity<participante_telefono>()
				.Property(e => e.telefono)
				.IsUnicode(false);

			modelBuilder.Entity<participante_telefono>()
				.Property(e => e.extension)
				.IsUnicode(false);

			modelBuilder.Entity<participante_telefono>()
				.Property(e => e.operadora_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<premio>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<premio>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<premio>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<premio>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<premio>()
				.Property(e => e.url_imagen_small)
				.IsUnicode(false);

			modelBuilder.Entity<premio>()
				.Property(e => e.url_imagen_large)
				.IsUnicode(false);

			modelBuilder.Entity<premio>()
				.Property(e => e.categoria_premio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<premio>()
				.Property(e => e.proveedor_premios_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<premio>()
				.Property(e => e.marca_premio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<premio>()
				.Property(e => e.operadora_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<premio>()
				.Property(e => e.tipo_envio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<premio>()
				.HasMany(e => e.error_recarga_sin_saldo_suficiente)
				.WithRequired(e => e.premio)
				.HasForeignKey(e => e.premio_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<premio>()
				.HasMany(e => e.transaccion_premio)
				.WithRequired(e => e.premio)
				.HasForeignKey(e => e.premio_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<producto>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<producto>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<producto>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<producto>()
				.Property(e => e.clave_ean)
				.IsUnicode(false);

			modelBuilder.Entity<producto>()
				.Property(e => e.clave_dun)
				.IsUnicode(false);

			modelBuilder.Entity<producto>()
				.Property(e => e.temporada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.Property(e => e.rama_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.Property(e => e.categoria_producto_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.Property(e => e.linea_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.Property(e => e.familia_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.Property(e => e.color_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.Property(e => e.talla_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<producto>()
				.HasOptional(e => e.producto1)
				.WithRequired(e => e.producto2);

			modelBuilder.Entity<producto>()
				.HasMany(e => e.transaccion_detalle)
				.WithOptional(e => e.producto)
				.HasForeignKey(e => e.producto_id);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.referencia_campania)
				.IsUnicode(false);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.datos_facturacion)
				.IsUnicode(false);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.usuario_alta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.usuario_cambio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<proveedor_premios>()
				.Property(e => e.usuario_baja_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<proveedor_premios>()
				.HasMany(e => e.premio)
				.WithOptional(e => e.proveedor_premios)
				.HasForeignKey(e => e.proveedor_premios_id);

			modelBuilder.Entity<rama>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<rama>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<rama>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<rama>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<rama>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<saldos_participante>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<saldos_participante>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<saldos_participante>()
				.Property(e => e.categoria_transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<sexo>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<sexo>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<sexo>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<sexo>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<sexo>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<sexo>()
				.HasMany(e => e.participante)
				.WithOptional(e => e.sexo)
				.HasForeignKey(e => e.sexo_id);
			
			modelBuilder.Entity<tema>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tema>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tema>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tema>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tema>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status>()
				.HasMany(e => e.participante_direccion)
				.WithRequired(e => e.status)
				.HasForeignKey(e => e.status_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<status_carga>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_carga>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status_carga>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status_carga>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status_carga>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_llamada>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_llamada>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status_llamada>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status_llamada>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status_llamada>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_llamada>()
				.HasMany(e => e.agenda_llamadas)
				.WithRequired(e => e.status_llamada)
				.HasForeignKey(e => e.status_llamada_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.cambia_status_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.acumula_codigo_mensaje)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.acumula_mensaje)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.redime_codigo_mensaje)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.redime_mensaje)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.acumula_alerta)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.redime_alerta)
				.IsUnicode(false);

			modelBuilder.Entity<status_participante>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_participante>()
				.HasMany(e => e.participante)
				.WithOptional(e => e.status_participante)
				.HasForeignKey(e => e.status_participante_id);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.usuario_alta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.usuario_cambio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.usuario_baja_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.clave_rms)
				.IsUnicode(false);

			modelBuilder.Entity<status_premio>()
				.Property(e => e.descripcion_rms)
				.IsUnicode(false);

			modelBuilder.Entity<status_premio>()
				.HasMany(e => e.historial_status_premio)
				.WithRequired(e => e.status_premio)
				.HasForeignKey(e => e.status_premio_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<status_seguimiento>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_seguimiento>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status_seguimiento>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status_seguimiento>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status_seguimiento>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_seguimiento>()
				.HasMany(e => e.llamada_seguimiento)
				.WithRequired(e => e.status_seguimiento)
				.HasForeignKey(e => e.status_seguimiento_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<status_tarjeta>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_tarjeta>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status_tarjeta>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status_tarjeta>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status_tarjeta>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_tarjeta>()
				.HasMany(e => e.historico_tarjetas)
				.WithRequired(e => e.status_tarjeta)
				.HasForeignKey(e => e.status_tarjeta_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<status_transaccion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<status_transaccion>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<status_transaccion>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<status_transaccion>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<status_transaccion>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<talla>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<talla>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<talla>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<talla>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<talla>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<temporada>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<temporada>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<temporada>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<temporada>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<temporada>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_agente>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_agente>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_agente>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_agente>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_agente>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_direccion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_direccion>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_direccion>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_direccion>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_direccion>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_direccion>()
				.HasMany(e => e.participante_direccion)
				.WithRequired(e => e.tipo_direccion)
				.HasForeignKey(e => e.tipo_direccion_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<tipo_llamada>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_llamada>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_llamada>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_llamada>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_llamada>()
				.Property(e => e.categoria_tipo_llamada_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_llamada>()
				.HasMany(e => e.agenda_llamadas)
				.WithRequired(e => e.tipo_llamada)
				.HasForeignKey(e => e.tipo_llamada_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<tipo_llamada>()
				.HasMany(e => e.llamada_seguimiento)
				.WithOptional(e => e.tipo_llamada)
				.HasForeignKey(e => e.tipo_llamada_id);

			modelBuilder.Entity<tipo_pago>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_pago>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_pago>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_pago>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_pago>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_participante>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_participante>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_participante>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_participante>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_participante>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_participante>()
				.HasMany(e => e.participante)
				.WithOptional(e => e.tipo_participante)
				.HasForeignKey(e => e.tipo_participante_id);

			modelBuilder.Entity<tipo_tarjeta>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_tarjeta>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_tarjeta>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_tarjeta>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_tarjeta>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_tarjeta>()
				.HasMany(e => e.historico_tarjetas)
				.WithRequired(e => e.tipo_tarjeta)
				.HasForeignKey(e => e.tipo_tarjeta_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<tipo_telefono>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_telefono>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_telefono>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_telefono>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_telefono>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_telefono>()
				.HasMany(e => e.participante_telefono)
				.WithRequired(e => e.tipo_telefono)
				.HasForeignKey(e => e.tipo_telefono_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<tipo_transaccion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_transaccion>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_transaccion>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_transaccion>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<tipo_transaccion>()
				.Property(e => e.categoria_transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tipo_transaccion>()
				.HasMany(e => e.transaccion)
				.WithRequired(e => e.tipo_transaccion)
				.HasForeignKey(e => e.tipo_transaccion_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<tmp_agentes>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_cajas>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_catproductos>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_formaspagos>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_sucursales>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_tickets>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_tickets>()
				.Property(e => e.Foliosbx)
				.IsUnicode(false);

			modelBuilder.Entity<tmp_tickets>()
				.HasMany(e => e.tmp_tickets_formasdepago)
				.WithOptional(e => e.tmp_tickets)
				.HasForeignKey(e => e.ticket_id);

			modelBuilder.Entity<tmp_tickets>()
				.HasMany(e => e.tmp_tickets_productos)
				.WithOptional(e => e.tmp_tickets)
				.HasForeignKey(e => e.ticket_id);

			modelBuilder.Entity<tmp_tickets_formasdepago>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_tickets_formasdepago>()
				.Property(e => e.ticket_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_tickets_productos>()
				.Property(e => e.Id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<tmp_tickets_productos>()
				.Property(e => e.ticket_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion>()
				.Property(e => e.tipo_transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion>()
				.Property(e => e.estado_cuenta_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion>()
				.Property(e => e.numero_autorizacion)
				.IsUnicode(false);

			modelBuilder.Entity<transaccion>()
				.Property(e => e.historico_ventas_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion>()
				.HasMany(e => e.error_recarga_sin_saldo_suficiente)
				.WithRequired(e => e.transaccion)
				.HasForeignKey(e => e.transaccion_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<transaccion>()
				.HasMany(e => e.transaccion_detalle)
				.WithRequired(e => e.transaccion)
				.HasForeignKey(e => e.transaccion_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<transaccion>()
				.HasMany(e => e.transaccion_premio)
				.WithRequired(e => e.transaccion)
				.HasForeignKey(e => e.transaccion_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<transaccion>()
				.HasMany(e => e.transaccion_comentarios)
				.WithRequired(e => e.transaccion)
				.HasForeignKey(e => e.transaccion_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<transaccion_comentarios>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_comentarios>()
				.Property(e => e.transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_detalle>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_detalle>()
				.Property(e => e.transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_detalle>()
				.Property(e => e.producto_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_detalle>()
				.Property(e => e.historico_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_premio>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_premio>()
				.Property(e => e.transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_premio>()
				.Property(e => e.premio_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_premio>()
				.Property(e => e.observaciones)
				.IsUnicode(false);

			modelBuilder.Entity<transaccion_premio>()
				.Property(e => e.folio_confirmacion)
				.IsUnicode(false);

			modelBuilder.Entity<transaccion_premio>()
				.HasMany(e => e.error_recarga_sin_saldo_suficiente)
				.WithRequired(e => e.transaccion_premio)
				.HasForeignKey(e => e.transaccion_premio_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<transaccion_premio>()
				.HasMany(e => e.historial_status_premio)
				.WithRequired(e => e.transaccion_premio)
				.HasForeignKey(e => e.transaccion_premio_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<vendedor>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<vendedor>()
				.Property(e => e.clave)
				.IsUnicode(false);

			modelBuilder.Entity<vendedor>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<vendedor>()
				.Property(e => e.descripcion_larga)
				.IsUnicode(false);

			modelBuilder.Entity<vendedor>()
				.Property(e => e.pais_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<bitacora_envios>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<bitacora_envios>()
				.Property(e => e.participante_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<bitacora_envios>()
				.Property(e => e.correo_electronico)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_envios>()
				.Property(e => e.asunto)
				.IsUnicode(false);

			modelBuilder.Entity<bitacora_envios>()
				.Property(e => e.mensaje)
				.IsUnicode(false);

			modelBuilder.Entity<nivel>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<nivel>()
				.Property(e => e.descripcion)
				.IsUnicode(false);

			modelBuilder.Entity<nivel>()
				.Property(e => e.distribuidor_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.codigo_postal)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.colonia)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.tipo_colonia)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.municipio)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.estado)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.ciudad)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.oficina_postal)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.clave_estado)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.clave_oficina)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.clave_codigo_postal)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.clave_tipo_colonia)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.clave_municipio)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.id_consulta_cp)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.zona)
				.IsUnicode(false);

			modelBuilder.Entity<sepomex>()
				.Property(e => e.clave_ciudad)
				.IsUnicode(false);

			modelBuilder.Entity<transaccion_redencion>()
				.Property(e => e.id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_redencion>()
				.Property(e => e.transaccion_id)
				.HasPrecision(18, 0);

			modelBuilder.Entity<transaccion_redencion>()
				.Property(e => e.transaccion_premio_id)
				.HasPrecision(18, 0);
		}
	}
}
