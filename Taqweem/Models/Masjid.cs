using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Classes;
using Taqweem.ViewModels;

namespace Taqweem.Models
{
    public enum JuristicMethod
    {
        UniversityOfIslamicSciencesKarachi = 1,
        MuslimWorldLeague = 2,
        IslamicSocietyOfNorthAmerica = 3,
        UmmAlQuraUniversityMakkah = 4,
        EgyptianGeneralAuthorityOfSurvey = 5
    }

    public class MasjidCountDown
    {
        public string NextSalaah;
        public string CountDown;
        public string SalaahTime;
    }

    public enum SalaahTimesType
    {
        ScheduleTime = 1,
        DailyTime = 2
    }

    //    if (JMethod == 1) { FajrAngle = 18; IshaAngle = 18; txtJuristicM.Text = "University of Islamic Sciences, Karachi"; }
    //else if (JMethod == 2) { FajrAngle = 18; IshaAngle = 17; txtJuristicM.Text = "Muslim World League"; }
    //else if (JMethod == 3) { FajrAngle = 15; IshaAngle = 15; txtJuristicM.Text = "Islamic Society of North America"; }
    //else if (JMethod == 4) { FajrAngle = 18.5; IshaAngle = 0; txtJuristicM.Text = "Umm al-Qura University, Makkah"; }
    //else if (JMethod == 5) { FajrAngle = 19.5; IshaAngle = 17.5; txtJuristicM.Text = "Egyptian General Authority of Survey"; }
    //else { FajrAngle = 18; IshaAngle = 18; txtJuristicM.Text = "N/A"; }

    public class Masjid : AuditableEntity
    {
        public int OldSiteId { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public DateTime LastUpdate { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Height { get; set; } = 0;

        public string TimeZoneId { get; set; } = "UTC";

        public virtual TimeZone TimeZone { get; set;}

        public string TZTimeZone { get; set; }

        public double TimeZoneDiff { get; set; }

        public JuristicMethod JuristMethod { get; set; } = JuristicMethod.UniversityOfIslamicSciencesKarachi;

        public bool LadiesFacility { get; set; }

        public bool JummahFacility { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string GeneralInfo { get; set; }

        public bool AllowRegistration { get; set; } = true;

        public int MaghribAdhaanDelay { get; set; } = 3;

        public SalaahTimesType SalaahTimesType { get; set; } = SalaahTimesType.ScheduleTime;

        public bool IsSpecialDayEnabled { get; set; }

        public bool IsPublicHolidaySpecialTimesEnabled { get; set; }

        public int SpecialDayNumber { get; set; }

        [NotMapped]
        public double Distance { get; set; }

        [NotMapped]
        public virtual MasjidCountDown CountDown { get; set; }

        public virtual ICollection<SalaahTime> SalaahTimes { get; set; }

        public virtual ICollection<Notice> Notices { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Masjid()
        {

        }
    }
}
