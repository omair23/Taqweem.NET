using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taqweem.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        string SendEmailString(string email, string subject, string message);
    }
}
