using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Taqweem.Models;

namespace Taqweem.ViewModels.ManageViewModels
{
    public class NoticesViewModel
    {
        [StringLength(38)]
        public string MasjidId { get; set; }

        public List<Notice> Notices { get; set; }
    }
}
