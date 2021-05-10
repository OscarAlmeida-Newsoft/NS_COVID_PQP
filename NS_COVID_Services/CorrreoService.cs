using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NS_COVID_Services
{
    public class CorrreoService
    {
        public void EnviaCorreo(List<string> Destinatarios, string Asunto, string CuerpoEmail)
        {
            try
            {
                SmtpClient mailClient = new SmtpClient();
                mailClient.Port = 25;
                mailClient.Host = "10.125.17.36";
                mailClient.EnableSsl = false;

                MailMessage correo = new MailMessage();

                var mail = ConfigurationManager.AppSettings["DireccionEnvioCorreo"].ToString();
                //var password = ConfigurationManager.AppSettings["ContraseñaEnvioCorreo"].ToString();

                correo.From = new MailAddress(mail, "Alerta registro covid");
                //NetworkCredential credentials = new NetworkCredential(mail, password);
                //mailClient.Credentials = credentials;

                if (Destinatarios != null && Destinatarios.Count > 0)
                {
                    foreach (string item in Destinatarios)
                    {
                        correo.To.Add(new MailAddress(item));
                    }
                }

                correo.Subject = Asunto;
                correo.Body = CuerpoEmail;
                correo.IsBodyHtml = true;

                mailClient.Send(correo);


            }
            catch (Exception ex)
            {

            }
        }
    }
}
