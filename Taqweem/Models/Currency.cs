using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Classes;

namespace Taqweem.Models
{
    public enum CurrencySymbolStyle
    {
        Prefix = 0,
        Suffix = 1
    }

    [Table("Currency", Schema = "World")]
    public class Currency : AuditableEntity
    {
        [Required]
        [Display(Name = "Currency Code")]
        public string Code { get; set; }

        public CurrencySymbolStyle Style { get; set; } = CurrencySymbolStyle.Prefix;

        public string Symbol { get; set; }

        public string Flag { get; set; }

        [Required]
        [Display(Name = "Currency Name")]
        public string Name { get; set; }

        public int NumberToBasic { get; set; } = 100;

        public string FractionalUnit { get; set; }

        public string Locations { get; set; }

        // Remember that this is from Fulcrum's base currency which is now set to the US Dollar, if it changes adapt accordingly
        public double ConversionRate { get; set; } = 0;
    }
}
