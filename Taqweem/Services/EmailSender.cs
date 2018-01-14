using Mailjet.Client;
using Mailjet.Client.Resources;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Taqweem.Classes;

namespace Taqweem.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly RazorViewToStringRenderer _renderer;
        private readonly MailboxAddress _sender;

        public EmailSender(RazorViewToStringRenderer renderer)
        {
            _renderer = renderer;
            _sender = new MailboxAddress("Taqweem", "taqweemmasjid@gmail.com");
        }

        public string EmailBody(string message)
        {
            EmailModel Model = new EmailModel();

            Model.Content = message;

            string content = _renderer.RenderViewToString(Model.EmailTemplate, Model);

            return content;
        }

        //public async Task SendEmailAsync(string email, string subject, string body)
        //{
        //    using (var client = new System.Net.Mail.SmtpClient())
        //    {
        //        client.Host = "smtp.gmail.com";
        //        client.Port = 587;
        //        client.EnableSsl = true;

        //        NetworkCredential NC = new NetworkCredential();
        //        NC.UserName = "taqweemmasjid@gmail.com";
        //        NC.Password = "Taqweem@786";
        //        client.Credentials = NC;

        //        MailAddress from = new MailAddress(NC.UserName, "Taqweem");

        //        //TO DO Remove Static Email Address
        //        MailAddress receiver = new MailAddress("omair334@gmail.com", "Omair Kazi"); //new MailAddress(email);

        //        MailMessage Mymessage = new MailMessage(from, receiver);

        //        Mymessage.Subject = subject;
        //        Mymessage.IsBodyHtml = true;

        //        Mymessage.Body = EmailBody(message);

        //        client.Send(Mymessage);
        //    }

        //    //return Task.CompletedTask;
        //}

        public Task SendEmailAsync(string email, string subject, string content)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                message.From.Add(_sender);

                message.To.Add(new MailboxAddress("omair334@gmail.com"));

                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = EmailBody(content);
                message.Body = bodyBuilder.ToMessageBody();

                MailjetClient client = new MailjetClient("e4c449d5c56e09c5f3b49f74ef60f230", "dc3c91fac5cee339cdd35aa26f323230");
                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                            .Property(Send.FromEmail, "TaqweemMasjid@gmail.com")
                            .Property(Send.FromName, "Taqweem")
                            .Property(Send.Subject, subject)
                            .Property(Send.HtmlPart, EmailBody(content))
                            .Property(Send.Recipients, "omair334@gmail.com");
                //new JArray {
                //                    new JObject {
                //                     {"Email", "passenger@mailjet.com"}
                //                     }
                //                });
                MailjetResponse response = client.PostAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    return Task.FromResult(0);
                }
                else
                {
                    return Task.FromResult(-1);
                }

                //using (var client = new SmtpClient())
                //{
                //    //client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //    client.Connect("in-v3.mailjet.com", 587, SecureSocketOptions.StartTls);

                //    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                //    //client.Authenticate("taqweemmasjid@gmail.com", "Taqweem@786");
                //    client.Authenticate("e4c449d5c56e09c5f3b49f74ef60f230", "dc3c91fac5cee339cdd35aa26f323230");

                //    client.Send(message);
                //    client.Disconnect(true);
                //}

                //return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                return Task.FromResult(-1);
            }
        }


        //public Task SendEmailAsync(string email, string subject, string message)
        //{
        //    return Task.CompletedTask;
        //}
    }
}
