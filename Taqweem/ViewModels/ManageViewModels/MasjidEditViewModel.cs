using System.ComponentModel.DataAnnotations;
using Taqweem.Models;

namespace Taqweem.ViewModels.ManageViewModels
{
    public class MasjidEditViewModel
    {
        public int OldSiteId { get; set; }

        [StringLength(38)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Height { get; set; }

        public string TimeZoneId { get; set; } = "South Africa Standard Time";

        public string TZTimeZone { get; set; }

        public double TimeZoneDiff { get; set; }

        public virtual Models.TimeZone TimeZone { get; set; }

        public JuristicMethod JuristMethod { get; set; }

        [Display(Name = "Has a Ladies Facility")]
        public bool LadiesFacility { get; set; }

        [Display(Name = "Jummah is Performed at this Masjid")]
        public bool JummahFacility { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string GeneralInfo { get; set; }

        [Display(Name = "Allow New Admins to Register")]
        public bool AllowRegistration { get; set; }

        public int MaghribAdhaanDelay { get; set; }

        public SalaahTimesType SalaahTimesType { get; set; }

        [Display(Name = "Masjid Uses Special Times on Public Holidays")]
        public bool IsPublicHolidaySpecialTimesEnabled { get; set; }

        [Display(Name = "Masjid Uses Special Times on a Particular Day Specified Further Below")]
        public bool IsSpecialDayEnabled { get; set; }

        [Display(Name = "Day of the Week that is the Special Day. Sunday - 0 Saturday - 6")]
        [Range(0, 6, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int SpecialDayNumber { get; set; }

        public double Distance { get; set; }
    }
}
