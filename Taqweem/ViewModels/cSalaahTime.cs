using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taqweem.ViewModels
{
    public class cSalaahTime
    {
        public DateTime FajrAdhaan { get; set; } = new DateTime(1, 1, 1, 3, 1, 0);

        public DateTime FajrSalaah { get; set;}

        public DateTime DhuhrAdhaan { get; set; }

        public DateTime DhuhrSalaah { get; set; }

        public DateTime AsrAdhaan { get; set; }

        public DateTime AsrSalaah { get; set; }

        public DateTime IshaAdhaan { get; set; }

        public DateTime IshaSalaah { get; set; }
    }
}
