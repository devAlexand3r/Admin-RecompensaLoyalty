using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JulioLoyalty.Business.EmailService
{
    public class Templates
    {
        private int idTemplate { get; set; }
        private string path { get; set; }
        private string content { get; set; }

        public Templates(int _idTemplate)
        {
            this.idTemplate = _idTemplate;
        }

        public string getTemplate()
        {
            try
            {
                path = HttpContext.Current.Server.MapPath($"/Templates/Template_{idTemplate}.html");

                if (!File.Exists(this.path))
                    return content;

                using (StreamReader stream = new StreamReader(path))
                {
                    content = stream.ReadToEnd();
                }
                return content;
            }
            catch (Exception)
            {
                return content;
            }
        }
    }
}
