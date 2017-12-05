using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taqweem.ViewModels
{
    public class cPerpetualTime
    {
        public DateTime SehriEnds { get; set; } = new DateTime(1, 1, 1, 3, 1, 0);

        public DateTime Fajr { get; set;}

        public DateTime Sunrise { get; set;}

        public DateTime Ishraaq { get; set;}

        public DateTime Zawaal { get; set;}

        public DateTime Dhuhr { get; set;}

        public DateTime AsrShafi { get; set; }

        public DateTime AsrHanafi { get; set; }

        public DateTime Sunset { get; set;}

        public DateTime Maghrib { get; set; }

        public DateTime Isha { get; set;}
    }
}
