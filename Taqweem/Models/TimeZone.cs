namespace Taqweem.Models
{
    public class TimeZone
    {
        public string Id { get; set; }

        public string StandardName { get; set; }

        public string DisplayName { get; set; }

        public string DaylightName { get; set; }

        public bool SupportsDaylightSavingTime { get; set; }

        public double DefaultUTCDifference { get; set; }
    }
}
