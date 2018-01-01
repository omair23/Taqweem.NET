using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Taqweem.Services;

namespace Taqweem.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            string Content = "<p>Dear Taqweem User</p><br><p>Thank you for signing up as a Masjid Administrator.</p><br>" +
                             "<p>Your details have been submitted successfully. There is only one step left: to activate your account.</p><br>" +
                              $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>" +
                              "<br><br><p>Shukran</p><p><strong>Taqweem Team</strong></p>";

            return emailSender.SendEmailAsync(email, "Taqweem Account Confirmation", Content);
        }
    }
}
