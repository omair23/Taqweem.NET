using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using Taqweem.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Taqweem.Models;
using Taqweem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Taqweem.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly RazorViewToStringRenderer _renderer;
        private readonly MailboxAddress _sender;
        private readonly AuthMessageSenderOptions _optionsAccessor;
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;
        private readonly EFRepository Repository;
        private readonly ApplicationDbContext _context;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, 
                            RazorViewToStringRenderer renderer,
                            IConfiguration Configuration,
                            IHostingEnvironment Env,
                            ApplicationDbContext context)
        {
            env = Env;
            configuration = Configuration;

            _optionsAccessor = optionsAccessor.Value;
            _renderer = renderer;
            //_sender = new MailboxAddress("Taqweem", "TaqweemMasjid@gmail.com");
            _sender = new MailboxAddress(configuration.GetValue<string>("MailboxAddressName"), configuration.GetValue<string>("MailboxAddress"));

            _context = context;
            Repository = new EFRepository(_context);
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

                if (env.IsProduction())
                {
                    //message.To.Add(new MailboxAddress("omair334@gmail.com"));
                    //message.To.Add(new MailboxAddress("Omair.Kazi@kpmg.co.za"));
                    message.To.Add(new MailboxAddress(email));
                }
                else
                {
                    message.To.Add(new MailboxAddress("omair334@gmail.com"));
                }

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

        public Task SendMultipleEmailsAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }

        //public Task SendEmailAsync(string email, string subject, string message)
        //{
        //    return Task.CompletedTask;
        //}

        public bool SendReport()
        {
            try
            {
                EmailModel Model = ReportEmailModel();

                SendEmailAsync("omair334@gmail.com", "Taqweem Email Report", Model.Content);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public EmailModel ReportEmailModel()
        {
            var Masjids = Repository
                            .GetAll<Masjid>()
                            .Include(s => s.Users)
                            .Where(s => s.Users.Count > 0)
                            .OrderByDescending(s => s.Users.Count)
                            .Take(7)
                            .ToList();

            string Text = "<h2 style='color:#1FA154;'>Taqweem Insights Report</h2>";

            Text += "<h3 style='color:#1FA154;'>Masjids with most users - Top 7</h3> <table border='1' class='table table-bordered table-striped'>" +
                "<tr><th>Name</th><th>Town</th><th>Country</th><th>Count</th></tr>";

            foreach (var Masjid in Masjids)
            {
                Text += "<tr><td>" + Masjid.Name + "</td><td>" + Masjid.Town + "</td><td>" + Masjid.Country + "</td><td>" + Masjid.Users.Count + "</td></tr>";
            }

            Text += "</table>";

            var Users = Repository.Find<ApplicationUser>(s => s.LastLogin >= DateTime.Now.AddDays(-7))
                        .Include(s => s.Masjid)
                        .OrderByDescending(s => s.LastLogin)
                        .ToList();

            Text += "<h3 style='color:#1FA154;'>Users logged on in the last 7 days</h3> <table border='1' class='table table-bordered table-striped'>" +
                "<tr><th>Email</th><th>Full Name</th><th>Last Login</th><th>Masjid</th><th>Town</th><th>Country</th></tr>";

            foreach (var User in Users)
            {
                Text += "<tr><td>" + User.Email + "</td><td>" + User.FullName + "</td><td>" + User.LastLogin.ToString("dd/MM/yyyy HH:mm") +
                    "</td><td>" + User.Masjid.Name + "</td><td>" + User.Masjid.Town + "</td><td>" + User.Masjid.Country + "</td></tr>";
            }

            Text += "</table>";

            var Users2 = Repository.Find<ApplicationUser>(s => s.CreatedAt >= DateTime.Now.AddDays(-7))
                        .Include(s => s.Masjid)
                        .OrderByDescending(s => s.CreatedAt)
                        .ToList();

            Text += "<h3 style='color:#1FA154;'>Users created in the last 7 days</h3> <table border='1' class='table table-bordered table-striped'>" +
                "<tr><th>Email</th><th>Full Name</th><th>Created At</th><th>Last Login</th><th>Masjid</th><th>Town</th><th>Country</th></tr>";

            foreach (var User in Users2)
            {
                Text += "<tr><td>" + User.Email + "</td><td>" + User.FullName + "</td><td>" + User.LastLogin.ToString("dd/MM/yyyy HH:mm") +
                    "</td><td>" + User.CreatedAt.ToString("dd/MM/yyyy HH:mm") +
                    "</td><td>" + User.Masjid.Name + "</td><td>" + User.Masjid.Town + "</td><td>" + User.Masjid.Country + "</td></tr>";
            }

            Text += "</table>";

            var Masjids2 = Repository.Find<Masjid>(s => s.CreatedAt >= DateTime.Now.AddDays(-7))
                                    .OrderByDescending(s => s.CreatedAt)
                                    .ToList();

            Text += "<h3 style='color:#1FA154;'>Masjids created in the last 7 days</h3> <table border='1' class='table table-bordered table-striped'>" +
                "<tr><th>Name</th><th>Town</th><th>Country</th><th>Created At</th></tr>";

            foreach (var Masjid in Masjids2)
            {
                Text += "<tr><td>" + Masjid.Name + "</td><td>" + Masjid.Town + "</td><td>" + Masjid.Country + "</td><td>" + Masjid.CreatedAt.ToString("dd/MM/yyyy HH:mm") + "</td></tr>";
            }

            Text += "</table>";

            var Notices = Repository.Find<Notice>(s => s.CreatedAt >= DateTime.Now.AddDays(-7))
                                    .Include(m => m.Masjid)
                                    .Include(s => s.Created)
                                    .OrderByDescending(s => s.CreatedAt)
                                    .ToList();

            Text += "<h3 style='color:#1FA154;'>Notices created in the last 7 days</h3> <table border='1' class='table table-bordered table-striped'>" +
                "<tr><th>Notice</th><th>Name</th><th>Town</th><th>Country</th><th>Created At</th><th>Created By</th></tr>";

            foreach (var Notice in Notices)
            {
                Text += "<tr><td>" + Notice.NoticeContent + "</td><td>" + Notice.Masjid.Name + "</td><td>" +
                    Notice.Masjid.Town + "</td><td>" + Notice.Masjid.Country + "</td><td>" +
                    Notice.Masjid.CreatedAt.ToString("dd/MM/yyyy HH:mm") + "</td><td>" + Notice.Created.FullName + "</td></tr>";
            }

            Text += "</table>";

            EmailModel Model = new EmailModel();

            Model.Content = Text;

            return Model;
        }
    }
}
