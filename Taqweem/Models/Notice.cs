using System;
using Taqweem.Classes;

namespace Taqweem.Models
{
    public class Notice : AuditableEntity
    {
        public string NoticeContent { get; set; }

        public bool IsHidden { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public string CreatedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    }
}
