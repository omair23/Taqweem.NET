using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Models;

namespace Taqweem.Classes
{
    public class MasjidDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public DateTime LastUpdate { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Height { get; set; } = 0;

        public string TimeZoneId { get; set; } = "UTC";

        public string TZTimeZone { get; set; }

        public JuristicMethod JuristMethod { get; set; } = JuristicMethod.UniversityOfIslamicSciencesKarachi;

        public bool LadiesFacility { get; set; }

        public bool JummahFacility { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string GeneralInfo { get; set; }

        public int MaghribAdhaanDelay { get; set; } = 3;

        public SalaahTimesType SalaahTimesType { get; set; } = SalaahTimesType.ScheduleTime;

        public bool IsSpecialDayEnabled { get; set; }

        public bool IsPublicHolidaySpecialTimesEnabled { get; set; }

        public int SpecialDayNumber { get; set; }

    }
}
