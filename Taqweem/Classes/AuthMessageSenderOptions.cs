
namespace Taqweem.Classes
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }

        public string SendGridPassword { get; set; }

        public string SendGridKey { get; set; }

        public bool IsDebug { get; set; } = false;

        public string DebugEmail { get; set; } = "TaqweemMasjid@gmail.com";
    }
}
