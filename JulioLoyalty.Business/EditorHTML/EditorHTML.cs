using JulioLoyalty.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business
{
    public class EditorHTML : IEditorHTML
    {
        ResultJson result = new ResultJson();
        public async Task<string> GetHTML(htmlPais html)
        {
            string _html = string.Empty;
            using (var db = new DbContextJulio())
            {
                var pais = await db.pais.FirstOrDefaultAsync();
                switch (html)
                {
                    case htmlPais.Aviso_de_privacidad:
                        {
                            _html = pais.Aviso_de_privacidad ?? string.Empty;
                        }
                        break;
                    case htmlPais.Terminos_y_condiciones:
                        {
                            _html = pais.Terminos_y_condiciones ?? string.Empty;
                        }
                        break;
                    case htmlPais.comofunciona_QUE_ES_RECOMPENSAS_LOYALTY:
                            _html = pais.Que_Es_Recompensas_Loyalty ?? string.Empty;
                        break;
                    case htmlPais.comofunciona_REGLAMENTO_DEL_PROGRAMA:
                            _html = pais.Reglamento_del_programa ?? string.Empty;
                        break;
                    case htmlPais.comofunciona_COMO_CANJEO_MIS_PUNTOS:
                            _html = pais.Como_canjeo_mis_puntos ?? string.Empty;
                        break;
                    default:
                        return string.Empty;
                        break;
                }
            }
            return _html;
        }

        public async Task<ResultJson> SaveHtml(htmlPais htmlTipe, string htmlText)
        {
            try
            {
                using (var db = new DbContextJulio())
                {
                    var pais = await db.pais.FirstOrDefaultAsync();
                    switch (htmlTipe)
                    {
                        case htmlPais.Aviso_de_privacidad:
                            {
                                pais.Aviso_de_privacidad = htmlText;
                            }
                            break;
                        case htmlPais.Terminos_y_condiciones:
                            {
                                pais.Terminos_y_condiciones = htmlText;
                            }
                            break;
                        case htmlPais.comofunciona_QUE_ES_RECOMPENSAS_LOYALTY:
                            {
                                pais.Que_Es_Recompensas_Loyalty = htmlText;
                            }
                            break;
                        case htmlPais.comofunciona_REGLAMENTO_DEL_PROGRAMA:
                            {
                                pais.Reglamento_del_programa = htmlText;
                            }
                            break;
                        case htmlPais.comofunciona_COMO_CANJEO_MIS_PUNTOS:
                            {
                                pais.Como_canjeo_mis_puntos = htmlText;
                            }
                            break;

                    }
                    db.Entry(pais).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }
    }

    public enum htmlPais
    {
        Aviso_de_privacidad,
        Terminos_y_condiciones,
        comofunciona_QUE_ES_RECOMPENSAS_LOYALTY,
        comofunciona_REGLAMENTO_DEL_PROGRAMA,
        comofunciona_COMO_CANJEO_MIS_PUNTOS,
    }
}
