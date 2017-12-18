using System;
using System.Collections.Generic;
using Taqweem.Classes;
using Taqweem.Models;

namespace Taqweem.ViewModels
{
    public class MasjidInfoViewModel
    {
        public Markers Markers { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public Masjid Masjid { get; set; }

        public cPerpetualTime PerpetualTime { get; set; }

        public cPerpetualTime NextPerpetualTime { get; set; }

        public SalaahTime SalaahTime { get; set; }

        public SalaahTime NextSalaahTime { get; set; }

        public List<Notice> Notices { get; set; }

        public double QiblaDistance { get; set; } = 0;

        public double QDNautical { get; set; }

        public double QDStatute { get; set; }

        public double QiblaBearing { get; set; }

        public MasjidInfoViewModel(Masjid pMasjid)
        {
            Markers = new Markers();

            Masjid = pMasjid;

            PerpetualTime = new cPerpetualTime(DateTime.Now, Masjid);

            //Static Conversions
            QiblaDistance = Math.Round(cCalculations.DistanceTo(Masjid.Latitude, Masjid.Longitude, 21.4225, 39.8262));

            QDStatute = Math.Round(QiblaDistance * 0.621371, 0);

            QDNautical = Math.Round(QiblaDistance * 0.539957, 0);

            QiblaBearing = Math.Round(cCalculations.DegreeBearing(Masjid.Latitude, Masjid.Longitude, 21.4225, 39.8262), 2);
        }

        
    }
}