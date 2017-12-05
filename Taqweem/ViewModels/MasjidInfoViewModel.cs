using System;
using System.Collections.Generic;

namespace Taqweem.ViewModels
{
    public class MasjidInfoViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public DateTime LastUpdate { get; set; }

        public MasjidInfoViewModel(string id)
        {
            Id = id;
            Name = "Masjid Muaadh bin Jabal Crosby";
            Town = "Johannesburg";
            Country = "South Africa";
            LastUpdate = new DateTime(2016, 9, 28);
        }
    }
}