using Taqweem.Models;

namespace Taqweem.ViewModels
{
    public class MasjidViewModel
    {
        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string TimeZoneId { get; set; }

        //public double TimeZone { get; set; }

        public bool LadiesFacility { get; set; }

        public bool JummahFacility { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string GeneralInfo { get; set; }

        public string SecurityQuestion { get; set; }
    }
}