using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taqweem.Models
{
    public enum UserStatus
    {
        Registered = 0,
        Active = 1,
        Pending = 2,
        Suspended = 3,
        Deleted = 4,
        Inactive = 5
    }

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool IsSuperUser { get; set; } = false;

        public bool ShowDetails { get; set; } = false;

        [Required]
        [EnumDataType(typeof(UserStatus))]
        public UserStatus ActiveStatus { get; set; } = UserStatus.Active;

        public string FullName { get; set; }

        public string MasjidId { get; set; }

        public virtual Masjid Masjid { get; set; }

        public DateTime LastLogin { get; set; }

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

        public ApplicationUser()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsSuperUser = false;
            ShowDetails = false;
        }
    }
}
