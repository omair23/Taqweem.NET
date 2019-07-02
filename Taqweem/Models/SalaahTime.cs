using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taqweem.Classes;

namespace Taqweem.Models
{
    public class SalaahTime // : AuditableEntity
    {
        public string MasjidId { get; set; }

        public SalaahTimesType Type { get; set; }

        public bool IsATimeChange { get; set; }

        public DateTime TimeDate { get; set; }

        public int DayNumber { get; set; }

        public bool IsFajrTimeChange { get; set; } = false;

        public DateTime FajrAdhaan { get; set; }

        public DateTime FajrSalaah { get; set; }

        public bool IsDhuhrTimeChange { get; set; } = false;

        public DateTime DhuhrAdhaan { get; set; }

        public DateTime DhuhrSalaah { get; set; }

        public bool IsJumuahTimeChange { get; set; } = false;

        public DateTime JumuahAdhaan { get; set; }

        public DateTime JumuahSalaah { get; set; }

        public bool IsSpecialDhuhrTimeChange { get; set; } = false;

        public DateTime SpecialDhuhrAdhaan { get; set; }

        public DateTime SpecialDhuhrSalaah { get; set; }

        public bool IsAsrTimeChange { get; set; } = false;

        public DateTime AsrAdhaan { get; set; }

        public DateTime AsrSalaah { get; set; }

        public bool IsIshaTimeChange { get; set; } = false;

        public DateTime IshaAdhaan { get; set; }

        public DateTime IshaSalaah { get; set; }

        //Stuff that's supposed to be inherited

        [Key]
        [StringLength(38)]
        [Column(Order = 0)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DefaultValue(null)]
        public DateTime? UpdatedAt { get; set; } = null;

        [DataType(DataType.Date)]
        [DefaultValue(null)]
        public DateTime? DeletedAt { get; set; } = null;

        [StringLength(38)]
        [DefaultValue(null)]
        public string DeletedBy { get; set; } = null;

        public SalaahTime()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
