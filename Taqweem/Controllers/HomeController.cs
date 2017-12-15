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
            List<Masjid> AllMasjids = Repository.GetAll<Masjid>().ToList();

            Markers Model = new Markers();

            Model.Marker = AllMasjids;

            return View(Model);
        }

        public IActionResult MasjidList()
        {
            List<Masjid> AllMasjids = Repository.GetAll<Masjid>().ToList();

            MasjidListViewModel Model = new MasjidListViewModel();

            Model.Masjids = AllMasjids;

            return View(Model);
        }

        public IActionResult MasjidInfo(string Id)
        {
            Masjid Info = Repository
                            .Find<Masjid>(s => s.Id == Id)
                            .FirstOrDefault();

            MasjidInfoViewModel Model = new MasjidInfoViewModel(Info);

            return View(Model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult AddMasjid()
        {
            Masjid masjid = new Masjid();
            return View(masjid);
        }

        public IActionResult PostAddMasjid(Masjid masjid)
        {
            Masjid Masjid = new Masjid();
            Masjid.Name = masjid.Name;
            Masjid.Town = masjid.Town;
            Masjid.Country = masjid.Country;

            Masjid.TimeZone = masjid.TimeZone;
            Masjid.Address = masjid.Address;
            Masjid.Contact = masjid.Contact;
            Masjid.GeneralInfo = masjid.GeneralInfo;
            Masjid.LadiesFacility = masjid.LadiesFacility;

            Masjid.Latitude = masjid.Latitude;
            Masjid.Longitude = masjid.Longitude;

            return Ok();
        }

        public IActionResult PerpetualCalendar(string Id)
        {
            Masjid Masjid = Repository
                            .Find<Masjid>(s => s.Id == Id)
                            .FirstOrDefault();

            cPerpCalendar Model = new cPerpCalendar(Masjid);

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
            List<Masjid> Markers = Repository.GetAll<Masjid>().ToList();

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
                    Id = u.Id,
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
