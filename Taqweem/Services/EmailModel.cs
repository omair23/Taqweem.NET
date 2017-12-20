namespace Taqweem.Services
{
    public class EmailModel
    {
        public string Sender { get; set; } = "omair334@gmail.com";

        public string Recipient { get; set; }

        public string Subject { get; set; }

        public string Header { get; set; }

        public string Content { get; set; }

        public string ContentWell { get; set; }

        public string Subheading { get; set; }

        public string EmailTemplate { get; set; } = "Shared/EmailTemplate";

        public bool useTaqweemLogo { get; set; } = true;
    }
}