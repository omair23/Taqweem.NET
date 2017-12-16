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

            //DB INIT
            if(AllMasjids.Count < 1)
            {
                List<Masjid> NewMasjids = new List<Masjid>();

                Masjid s = new Masjid();
                s.Name = "Masjid Muaadh bin Jabal - Crosby";
                s.Town = "Johannesburg";
                s.Country = "South Africa";
                s.Latitude = -26.195149;
                s.Longitude = 27.990238;
                s.TimeZone = 2;
                NewMasjids.Add(s);

                Masjid r = new Masjid();
                r.Name = "Masjid-e-Khair - Mayfair West";
                r.Town = "Johannesburg";
                r.Country = "South Africa";
                r.Latitude = -26.197940;
                r.Longitude = 27.997250;
                r.TimeZone = 2;
                NewMasjids.Add(r);

                Repository.AddMultiple(NewMasjids);

                AllMasjids = Repository.GetAll<Masjid>().ToList();
            }
            ////

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
            return View();
        }

        [HttpPost]
        public string PostAddMasjid(MasjidViewModel MasjidVM)
        {
            try
            {
                if (MasjidVM.SecurityQuestion != "6")
                    return "Fail - Security Question";

                Masjid Masjid = new Masjid();
                Masjid.Name = MasjidVM.Name;
                Masjid.Town = MasjidVM.Town;
                Masjid.Country = MasjidVM.Country;

                Masjid.TimeZone = MasjidVM.TimeZone;
                Masjid.Address = MasjidVM.Address;
                Masjid.Contact = MasjidVM.Contact;
                Masjid.GeneralInfo = MasjidVM.GeneralInfo;
                Masjid.LadiesFacility = MasjidVM.LadiesFacility;

                Masjid.Latitude = MasjidVM.Latitude;
                Masjid.Longitude = MasjidVM.Longitude;

                Repository.Add(Masjid);

                return "Successful";
            }
            catch(Exception ex)
            {
                return "Fail" + ex.Message;
            }

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
