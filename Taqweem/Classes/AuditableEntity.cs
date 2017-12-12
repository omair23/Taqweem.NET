using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taqweem.Classes
{
    public class AuditableEntity
    {
        [Key]
        [StringLength(38)]
        [Column(Order = 0)]
        public string Id { get; set; }

        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DefaultValue(null)]
        public DateTime? UpdatedAt { get; set; } = null;

        [DataType(DataType.Date)]
        [DefaultValue(null)]
        public DateTime? DeletedAt { get; set; } = null;

        public AuditableEntity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
