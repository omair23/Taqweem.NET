using System;
using Taqweem.Classes;

namespace Taqweem.Models
{
    public class SalaahTime : AuditableEntity
    {
        public string MasjidId { get; set; }

        public SalaahTimesType Type { get; set; }

        public bool IsATimeChange { get; set; }

        public DateTime TimeDate { get; set; }

        public int DayNumber { get; set; }

        public DateTime FajrAdhaan { get; set; }

        public DateTime FajrSalaah { get; set; }

        public DateTime DhuhrAdhaan { get; set; }

        public DateTime DhuhrSalaah { get; set; }

        public DateTime JumuahAdhaan { get; set; }

        public DateTime JumuahSalaah { get; set; }

        public DateTime SpecialDhuhrAdhaan { get; set; }

        public DateTime SpecialDhuhrSalaah { get; set; }

        public DateTime AsrAdhaan { get; set; }

        public DateTime AsrSalaah { get; set; }

        public DateTime IshaAdhaan { get; set; }

        public DateTime IshaSalaah { get; set; }
    }
}
