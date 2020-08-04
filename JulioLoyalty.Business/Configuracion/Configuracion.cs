using JulioLoyalty.Business.Parameters;
using JulioLoyalty.Model;
using JulioLoyalty.Model.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Configuracion
{
    public class Configuracion : IConfiguracion
    {
        public async Task<RequestPais> GetPaisAsync()
        {
            using (var db = new DbContextJulio())
            {
                var pais = await db.pais.FirstOrDefaultAsync();
                RequestPais requestPais = new RequestPais()
                {
                    id = pais.id,
                    clave = pais.clave,
                    descripcion = pais.descripcion,
                    descripcion_larga = pais.descripcion_larga,
                    correo_electronico = pais.correo_electronico,
                    remitente = pais.remitente,
                    servidor_pop = pais.servidor_pop,
                    servidor_smtp = pais.servidor_smtp,
                    usuario_correo = pais.usuario_correo,
                    password_correo = pais.password_correo,
                    prefijo_rms = pais.prefijo_rms,
                    usuario_rms = pais.usuario_rms,
                    password_rms = pais.password_rms,
                    valor_punto = pais.valor_punto,
                    url_programa = pais.url_programa,
                    clave_carga = pais.clave_carga,
                    url_logo = pais.url_logo,
                    tipo_envio_digital = pais.tipo_envio_digital,
                    tipo_envio_fisico = pais.tipo_envio_fisico,
                    tipo_envio_recarga = pais.tipo_envio_recarga,
                    banner_carousel = pais.banner_carousel,
                    theme = pais.theme
                };
                return requestPais;
            }
        }

        public async Task<RequestPais> UpdatePaisAsync(RequestPais pais)
        {
            using (var db = new DbContextJulio())
            {
                var _pais = await db.pais.FirstOrDefaultAsync(m => m.id == pais.id);
                _pais.clave = pais.clave;
                _pais.descripcion = pais.descripcion;
                _pais.descripcion_larga = pais.descripcion_larga;
                _pais.correo_electronico = pais.correo_electronico;
                _pais.remitente = pais.remitente;
                _pais.servidor_pop = pais.servidor_pop;
                _pais.servidor_smtp = pais.servidor_smtp;
                _pais.usuario_correo = pais.usuario_correo;
                _pais.password_correo = pais.password_correo;
                _pais.prefijo_rms = pais.prefijo_rms;
                _pais.usuario_rms = pais.usuario_rms;
                _pais.password_rms = pais.password_rms;
                _pais.valor_punto = pais.valor_punto;
                _pais.url_programa = pais.url_programa;
                _pais.clave_carga = pais.clave_carga;
                _pais.url_logo = pais.url_logo;
                _pais.tipo_envio_digital = pais.tipo_envio_digital;
                _pais.tipo_envio_fisico = pais.tipo_envio_fisico;
                _pais.banner_carousel = pais.banner_carousel;
                _pais.theme = pais.theme;

                db.pais.AddOrUpdate(_pais);
                var result = await db.SaveChangesAsync();
                return pais;
            }
        }
        public async Task<List<tema>> GetTemaListAsync()
        {
            using (var db = new DbContextJulio())
            {
                var temas = await db.tema.Where(m => m.fecha_baja == null).ToListAsync();
                return temas;
            }
        }
    }
}
