using System;
using System.Collections.Generic;

namespace Taqweem.ViewModels
{
    public class MasjidListViewModel
    {
        public List<MasjidList> Masjids { get; set; }

        public MasjidListViewModel()
        {
            Masjids = new List<MasjidList>();
        }
    }

    public class MasjidList
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public MasjidList(string id, string name, string town, string country)
        {
            Id = id;
            Name = name;
            Town = town;
            Country = country;
        }
    }
}