using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Taqweem.Models.ManageViewModels
{
    public class AdminDashboardViewModel
    {
        [DataType(DataType.Upload)]
        public IFormFile MyFile { get; set; }

        public int Users { get; set; }

        public int Masjids { get; set; }

        public int MasjidSalaahTimes { get; set; }

        public int MasjidNotices { get; set; }
    }
}
