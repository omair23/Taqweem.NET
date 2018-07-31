using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using Taqweem.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Taqweem.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly RazorViewToStringRenderer _renderer;
        private readonly MailboxAddress _sender;
        private readonly AuthMessageSenderOptions _optionsAccessor;
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, 
                            RazorViewToStringRenderer renderer,
                            IConfiguration Configuration,
                            IHostingEnvironment Env)
        {
            env = Env;
            configuration = Configuration;

            _optionsAccessor = optionsAccessor.Value;
            _renderer = renderer;
            //_sender = new MailboxAddress("Taqweem", "TaqweemMasjid@gmail.com");
            _sender = new MailboxAddress(configuration.GetValue<string>("MailboxAddressName"), configuration.GetValue<string>("MailboxAddress"));
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
            bool result = ActualSendEmail(email, subject, content);

            if (result == true)
                return Task.FromResult(0);
            else
                return Task.FromResult(-1);
        }

        public string SendEmailString(string email, string subject, string content)
        {
            bool result = ActualSendEmail(email, subject, content);

            if (result == true)
                return "Succeeded";
            else
                return "Failed";
        }

        public bool ActualSendEmail(string email, string subject, string content)
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

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.sendgrid.net", 587, SecureSocketOptions.Auto);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                    
                    client.Authenticate(configuration.GetValue<string>("SendGrid_User"), configuration.GetValue<string>("SendGrid_Password"));
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public Task SendEmailAsync(string email, string subject, string message)
        //{
        //    return Task.CompletedTask;
        //}
    }
}
