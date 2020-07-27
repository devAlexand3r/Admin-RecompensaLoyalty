using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.EmailService
{
    public class Email : IRepositoryEmail
    {
        private Templates template;
        private EParameters parameters;

        private string bodyEmail;
        public Email(Templates _template, EParameters _setting)
        {
            this.template = _template;
            this.parameters = _setting;
        }
        public void Submit()
        {
            bodyEmail = $"<h3>Hola @fullName </h3> <p>Accede a la siguiente URL para poder restablecer tu contraseña:</p> <p><a href='{parameters.Link}'>Cambiar contraseña</a></p>";
            Credentials cred = new Credentials();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(cred.Address, cred.DisplayName);
            mail.To.Add(parameters.Email);

            mail.Subject = cred.Subject;

            if (!string.IsNullOrEmpty(cred.Copy))
                mail.CC.Add(cred.Copy);

            if (!string.IsNullOrEmpty(cred.CopyHidden))
                mail.Bcc.Add(cred.CopyHidden);

            mail.IsBodyHtml = true;
            mail.Body = replaceKeys().ToString();
            mail.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = cred.Host;
            smtp.Credentials = new NetworkCredential(cred.UserName, cred.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(mail);
            smtp.Dispose();
        }

        private StringBuilder replaceKeys()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(bodyEmail);
            builder.Replace("@fullName", parameters.FullName);
            builder.Replace("@Email", parameters.Email);
            builder.Replace("@CurrentTime", parameters.currentTime.ToString());
            builder.Replace("@UserName", parameters.UserName);
            builder.Replace("@Password", parameters.Password);
            return builder;
        }

        public void SubmitExit()
        {
            bodyEmail =
                $"<h3>Hola @fullName, bienvenido a JULIO Loyalty</h3>" +
                $"<h4>Datos de sesión:</h3>" +
                $"<p>Usuario:  @UserName </p>" +
                $"<p>Contraseña: @Password </p>" +
                $"<p>Puedes inicar sesión <a href='{parameters.Link}'>Aquí</a></p>";
            Credentials cred = new Credentials();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(cred.Address, cred.DisplayName);
            mail.To.Add(parameters.Email);

            mail.Subject = "Bienvenido a JULIO Loyalty";

            if (!string.IsNullOrEmpty(cred.Copy))
                mail.CC.Add(cred.Copy);

            if (!string.IsNullOrEmpty(cred.CopyHidden))
                mail.Bcc.Add(cred.CopyHidden);

            mail.IsBodyHtml = true;
            mail.Body = replaceKeys().ToString();
            mail.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = cred.Host;
            smtp.Credentials = new NetworkCredential(cred.UserName, cred.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(mail);
            smtp.Dispose();
        }

        public void UnifySubmitExit()
        {
            bodyEmail =
                $"<h3>Hola @fullName, La unificación se realizo correctamente</h3>" +
                $"<h4>Datos de sesión:</h3>" +
                $"<p>Usuario:  @UserName </p>" +
                $"<p>Contraseña: @Password </p>" +
                $"<p>Puedes inicar sesión <a href='{parameters.Link}'>Aquí</a></p>";
            Credentials cred = new Credentials();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(cred.Address, cred.DisplayName);
            mail.To.Add(parameters.Email);

            mail.Subject = "UNIFICACIÓN EXITOSO";

            if (!string.IsNullOrEmpty(cred.Copy))
                mail.CC.Add(cred.Copy);

            if (!string.IsNullOrEmpty(cred.CopyHidden))
                mail.Bcc.Add(cred.CopyHidden);

            mail.IsBodyHtml = true;
            mail.Body = replaceKeys().ToString();
            mail.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = cred.Host;
            smtp.Credentials = new NetworkCredential(cred.UserName, cred.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(mail);
            smtp.Dispose();
        }
    }
}
