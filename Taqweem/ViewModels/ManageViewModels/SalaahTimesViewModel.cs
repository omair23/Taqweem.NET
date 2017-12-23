using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Taqweem.Models;

namespace Taqweem.ViewModels.ManageViewModels
{
    public class SalaahTimesViewModel
    {
        [StringLength(38)]
        public string MasjidId { get; set; }

        public List<SalaahTime> SalaahTimes { get; set; }

        public SalaahTimesType Type { get; set; }
    }

    public class SalaahTimeViewModel
    {
        [Key]
        public string KeyId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Fajr Adhaan")]
        public DateTime FajrAdhaan { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Fajr Salaah")]
        public DateTime FajrSalaah { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Dhuhr Adhaan")]
        public DateTime DhuhrAdhaan { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Dhuhr Salaah")]
        public DateTime DhuhrSalaah { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Jumuah Adhaan")]
        public DateTime JumuahAdhaan { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Jumuah Salaah")]
        public DateTime JumuahSalaah { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Asr Adhaan")]
        public DateTime AsrAdhaan { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Asr Salaah")]
        public DateTime AsrSalaah { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Isha Adhaan")]
        public DateTime IshaAdhaan { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Isha Salaah")]
        public DateTime IshaSalaah { get; set; }
    }
}
