using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Models;

namespace Taqweem.ViewModels
{
    public class Markers
    {
         public List<Masjid> Marker { get; set; }

        public Markers()
        {
            Marker = new List<Masjid>();
        }
    }
}
