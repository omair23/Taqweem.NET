using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Classes;

namespace Taqweem.Models
{

    [Table("PublicHoliday", Schema = "World")]
    public class PublicHoliday : AuditableEntity
    {
        public DateTime DayOfHoliday { get; set; }

        public string NameOfHoliday { get; set; }

        public string Country { get; set; }

        [StringLength(38)]
        public string CreatedId { get; set; } = null;

        public virtual ApplicationUser Created { get; set; }
    }
}
