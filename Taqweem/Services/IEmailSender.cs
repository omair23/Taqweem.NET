using System.Threading.Tasks;

namespace Taqweem.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        string SendEmailString(string email, string subject, string message);

        bool SendReport();

        EmailModel ReportEmailModel();
    }
}
