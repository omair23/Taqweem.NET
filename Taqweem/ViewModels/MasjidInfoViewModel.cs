using System;
using System.Collections.Generic;

namespace Taqweem.ViewModels
{
    public enum JuristicMethod
    {
        UniversityOfIslamicSciences = 1,

    }

    public class MasjidInfoViewModel
    {
        public string Id { get; set; }

        public cPerpetualTime PerpetualTime { get; set; }

        public cSalaahTime SalaahTime { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public DateTime LastUpdate { get; set; }

        public double TimeZone { get; set; } = 2;

        public double QiblaDistance { get; set; } = 0;

        public double QDNautical { get; set; }

        public double QDStatute { get; set; }

        public double QiblaBearing { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public JuristicMethod JuristicM { get; set; } = JuristicMethod.UniversityOfIslamicSciences;

        public bool LadiesFacility { get; set; }

        public bool JummahFacility { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public MasjidInfoViewModel(string id)
        {
            Id = id;
            Name = "Masjid Muaadh bin Jabal Crosby";
            Town = "Johannesburg";
            Country = "South Africa";
            LastUpdate = new DateTime(2016, 9, 28);

            PerpetualTime = new cPerpetualTime();
            SalaahTime = new cSalaahTime();


            //Static Conversions
            QDStatute = Math.Round(QiblaDistance * 0.621371, 0);

            QDNautical = Math.Round(QiblaDistance * 0.539957, 0);
        }
    }
}