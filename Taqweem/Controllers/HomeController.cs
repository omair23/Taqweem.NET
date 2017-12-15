using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taqweem.Models;
using Taqweem.ViewModels;
using System.Xml;
using Taqweem.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Taqweem.Data;

namespace Taqweem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EFRepository Repository;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            Repository = new EFRepository(_context);
        }

        public IActionResult Index()
        {
            Markers Model = new Markers();

            var Mas = Repository.GetAll<Masjid>().ToList();

            Model.Marker.Add(new Masjid("1", -28, 28, 0, 2));

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

        public double rad(double value)
        {
            return value * 180 / Math.PI;
        }

        public List<Masjid> NearestMasjids(double Latitude, double Longitude, int Radius)
        {
            List<Masjid> Markers = new List<Masjid>();

            Masjid X = new Masjid("1", -30, 28, 0, 2);
            X.Name = "ABC";
            X.Town = "DEF";
            X.Country = "GHI";
            Markers.Add(X);

            Masjid Y = new Masjid("1", -29, 28, 0, 2);
            Y.Name = "TER";
            Y.Town = "TER";
            Y.Country = "TER";
            Markers.Add(Y);

            List<Masjid> Nearest = new List<Masjid>();

            foreach (var Item in Markers)
            {
                var d = cCalculations.DistanceTo(Latitude, Longitude, Item.Latitude, Item.Longitude);

                if (d < Radius)
                {
                    Item.Distance = Math.Round(d, 2);

                    Nearest.Add(Item);
                }
            }

            return Nearest.OrderBy(s => s.Distance).ToList();
        }


        public IActionResult NearestMasjidsTable(double Latitude, double Longitude, int Radius)
        {
            try
            {
                List<Masjid> NearestM = NearestMasjids(Latitude, Longitude, Radius);

                var _json = NearestM.Select(u => new
                {
                    Masjid = u.Name + ", " + u.Town + ", " + u.Country,
                    Distance = u.Distance + " KM",
                    NextSalaah = "",
                    Countdown = "",
                    SalaahTime = "",
                    LadiesFacility = "",
                })
                .ToList();

                return Json(new { data = _json }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
