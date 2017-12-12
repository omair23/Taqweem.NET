using System;
using System.Collections.Generic;
using Taqweem.Classes;
using Taqweem.Models;

namespace Taqweem.ViewModels
{
    public class MasjidInfoViewModel
    {
        public Masjid Masjid { get; set; }

        public cPerpetualTime PerpetualTime { get; set; }

        public cSalaahTime SalaahTime { get; set; }

        public double QiblaDistance { get; set; } = 0;

        public double QDNautical { get; set; }

        public double QDStatute { get; set; }

        public double QiblaBearing { get; set; }

        public MasjidInfoViewModel(string id)
        {
            Masjid = new Masjid(id, -26.195149, 27.990238, 0, 2);

            Masjid.Name = "Masjid Muaadh bin Jabal Crosby";
            Masjid.Town = "Johannesburg";
            Masjid.Country = "South Africa";

            PerpetualTime = new cPerpetualTime(DateTime.Now, Masjid);
            SalaahTime = new cSalaahTime();

            //Static Conversions
            QiblaDistance = Math.Round(cCalculations.DistanceTo(Masjid.Latitude, Masjid.Longitude, 21.4225, 39.8262));

            QDStatute = Math.Round(QiblaDistance * 0.621371, 0);

            QDNautical = Math.Round(QiblaDistance * 0.539957, 0);
        }

        
    }
}