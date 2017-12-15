using System;
using System.Collections.Generic;
using Taqweem.Models;

namespace Taqweem.ViewModels
{
    public class MasjidListViewModel
    {
        public List<Masjid> Masjids { get; set; }

        public MasjidListViewModel()
        {
            Masjids = new List<Masjid>();
        }
    }
}