using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taqweem.Models;
using Taqweem.ViewModels;
using System.Xml;

namespace Taqweem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Markers Model = new Markers();

            Masjid X = new Masjid("1", -30, 28, 0, 2);
            X.Name = "ABC";
            X.Town = "DEF";
            X.Country = "GHI";

            Model.Marker.Add(X);

            Model.Marker.Add(new Masjid("1", 25, 28, 0, 2));

            return View(Model);
        }

        public IActionResult MasjidList()
        {
            MasjidListViewModel Model = new MasjidListViewModel();

            Model.Masjids.Add(new MasjidList("1", "Masjid Muaadh bin Jabal Crosby", "Johannesburg", "South Africa"));

            Model.Masjids.Add(new MasjidList("2", "Masjid Al Farooq", "Johannesburg", "South Africa"));

            Model.Masjids.Add(new MasjidList("2", "Masjid Al Farooq", "Johannesburg", "South Africa"));

            return View(Model);
        }

        public IActionResult MasjidInfo(string Id)
        {
            MasjidInfoViewModel Model = new MasjidInfoViewModel("TO DO replace");

            return View(Model);
        }

        public IActionResult PerpetualCalendar(string Id)
        {
            cPerpCalendar Model = new cPerpCalendar();

            return View(Model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
