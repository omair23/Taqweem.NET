using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Taqweem.Classes;

namespace Taqweem.Models
{
    public class Notice : AuditableEntity
    {
        public string NoticeContent { get; set; }

        public bool IsHidden { get; set; } = false;

        public string MasjidId { get; set; }

        public virtual Masjid Masjid { get; set; }

        [StringLength(38)]
        public string CreatedId { get; set; } = null;

        public virtual ApplicationUser Created { get; set; }
    }
}
