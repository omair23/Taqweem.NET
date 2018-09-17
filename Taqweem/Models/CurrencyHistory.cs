using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Classes;

namespace Taqweem.Models
{
    [Table("CurrencyHistory", Schema = "World")]
    public class CurrencyHistory : AuditableEntity
    {
        [Required]
        [Display(Name = "Currency Code")]
        public string Code { get; set; }

        public DateTime DateTimeStamp { get; set; }

        // Remember that this is from Fulcrum's base currency which is now set to the US Dollar, if it changes adapt accordingly
        public double ConversionRate { get; set; } = 0;
    }
}
