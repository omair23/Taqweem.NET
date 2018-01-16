using Mailjet.Client;
using Mailjet.Client.Resources;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Taqweem.Classes;

namespace Taqweem.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly RazorViewToStringRenderer _renderer;
        private readonly MailboxAddress _sender;
        private readonly AuthMessageSenderOptions _optionsAccessor;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, RazorViewToStringRenderer renderer)
        {
            _optionsAccessor = optionsAccessor.Value;
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

                using (var client = new SmtpClient())
                {
                    //client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Connect("smtp.sendgrid.net", 587, SecureSocketOptions.StartTls);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_optionsAccessor.SendGridUser, _optionsAccessor.SendGridPassword);

                    client.Send(message);
                    client.Disconnect(true);
                }

                return Task.FromResult(0);
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
