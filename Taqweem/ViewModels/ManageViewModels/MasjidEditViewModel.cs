using System.ComponentModel.DataAnnotations;
using Taqweem.Models;

namespace Taqweem.ViewModels.ManageViewModels
{
    public class MasjidEditViewModel
    {
        [StringLength(38)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Height { get; set; }

        public string TimeZoneId { get; set; } = "South Africa Standard Time";

        public virtual Models.TimeZone TimeZone { get; set; }

        public JuristicMethod JuristMethod { get; set; }

        public bool LadiesFacility { get; set; }

        public bool JummahFacility { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string GeneralInfo { get; set; }

        public bool AllowRegistration { get; set; }

        public int MaghribAdhaanDelay { get; set; }

        public SalaahTimesType SalaahTimesType { get; set; }
    }
}
