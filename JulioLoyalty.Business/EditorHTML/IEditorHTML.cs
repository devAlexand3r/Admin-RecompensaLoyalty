using System.Threading.Tasks;

namespace JulioLoyalty.Business
{
    public interface IEditorHTML
    {
        Task<string> GetHTML(htmlPais html);
        Task<ResultJson> SaveHtml(htmlPais htmlTipe, string htmlText);
    }
}