using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Taqweem.Classes;

namespace Taqweem.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly RazorViewToStringRenderer _renderer;

        public EmailSender(RazorViewToStringRenderer renderer)
        {
            _renderer = renderer;
        }

        public string EmailBody(string message)
        {
            EmailModel Model = new EmailModel();

            Model.Content = message;

            string content = _renderer.RenderViewToString(Model.EmailTemplate, Model);

            return content;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new System.Net.Mail.SmtpClient())
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                NetworkCredential NC = new NetworkCredential();
                NC.UserName = "omair000@gmail.com";
                NC.Password = "9409285082081";
                client.Credentials = NC;

                MailAddress from = new MailAddress(NC.UserName, "Taqweem");

                MailAddress receiver = new MailAddress("omair334@gmail.com", "Omair Kazi");
                MailMessage Mymessage = new MailMessage(from, receiver);
                Mymessage.Subject = subject;
                Mymessage.IsBodyHtml = true;

                Mymessage.Body = EmailBody(message);

                client.Send(Mymessage);
            }

            //return Task.CompletedTask;
        }

        //public Task SendEmailAsync(string email, string subject, string message)
        //{
        //    return Task.CompletedTask;
        //}
    }
}
