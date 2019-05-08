using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Taqweem.Models;
using Taqweem.ViewModels;
using Taqweem.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Taqweem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Taqweem.Services;

namespace Taqweem.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly EFRepository Repository;
        private readonly IEmailSender _emailSender;
        private readonly WorldService _worldService;
        public readonly TaqweemService _taqweemService;

        public HomeController(ApplicationDbContext context, 
                                UserManager<ApplicationUser> userManager, 
                                IEmailSender emailSender,
                                WorldService worldService,
                                TaqweemService taqweemService)
        {
            _worldService = worldService;
            _userManager = userManager;
            _context = context;
            Repository = new EFRepository(_context);
            _emailSender = emailSender;
            _taqweemService = taqweemService;
        }

        public IActionResult OldSiteRedirect(string Id)
        {
            try
            {
                int OldId = Convert.ToInt32(Id);

                Masjid masjid = _taqweemService.MasjidGetByOldSiteId(OldId);

                if (masjid != null)
                    return RedirectToAction("MasjidInfo", "Home", new { Id = masjid.Id });
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        

        public IActionResult Index()
        {
            List<Masjid> AllMasjids = _taqweemService.MasjidGetAll().ToList();

            Markers Model = new Markers();

            Model.Marker = AllMasjids;

            return View(Model);

            /*
            EmailModel Model = _emailSender.ReportEmailModel();

            return View("EmailTemplate", Model);
            */
        }

        public IActionResult CurrencyMatrix()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult MasjidList()
        {
            List<Masjid> AllMasjids = _taqweemService.MasjidGetAll().OrderBy(s => s.OldSiteId).ToList();

            MasjidListViewModel Model = new MasjidListViewModel();

            Model.Masjids = AllMasjids;

            return View(Model);
        }

        public IActionResult MasjidInfo(string Id)
        {
            Masjid Info = _taqweemService.MasjidGetByIdIncluded(Id);

            MasjidInfoViewModel Model = new MasjidInfoViewModel(Info);

            Model.Markers.Marker = _taqweemService.MasjidGetAll().ToList();

            Model.Users = _taqweemService.UsersGetByMasjidId(Id);

            Model.Notices = _taqweemService.NoticesGetByMasjidIdUnhidden(Id);

            Model.SalaahTime = GetSalaahTime(Info, DateTime.Now);

            Model.NextSalaahTime = NextSalaahTime(Info, DateTime.Now);

            if (Model.NextSalaahTime != null)
            {
                DateTime val = new DateTime(DateTime.Now.Year, 1, 1);
                val = val.AddDays(Model.NextSalaahTime.DayNumber - 1);

                if (Model.NextSalaahTime.DayNumber < DateTime.Now.DayOfYear)
                {
                    val = new DateTime(DateTime.Now.Year + 1, 1, 1);
                    val = val.AddDays(Model.NextSalaahTime.DayNumber - 1);
                }                

                Model.NextPerpetualTime = new cPerpetualTime(val, Info, false);
            }

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

                //Masjid.TimeZone = MasjidVM.TimeZone;
                Masjid.TimeZoneId = MasjidVM.TimeZoneId;

                Masjid.Address = MasjidVM.Address;
                Masjid.Contact = MasjidVM.Contact;
                Masjid.GeneralInfo = MasjidVM.GeneralInfo;
                Masjid.LadiesFacility = MasjidVM.LadiesFacility;

                Masjid.Latitude = MasjidVM.Latitude;
                Masjid.Longitude = MasjidVM.Longitude;

                Masjid = _taqweemService.MasjidCreate(Masjid);

                return "Successful";
            }
            catch(Exception ex)
            {
                return "Fail" + ex.Message;
            }

        }

        [HttpPost]
        public string PostSendFeedback(ContactViewModel CVM)
        {
            try
            {
                if (CVM.SecurityQuestion != "6")
                    return "Fail - Security Question";

                string Message = String.Format("Name: {0} <br>Email Address: {1} <br>Location: {2} <br>Message: {3}", CVM.Name, CVM.Email, CVM.Location, CVM.Message);

                _emailSender.SendEmailAsync("taqweemmasjid@gmail.com", "Taqweem Feedback", Message);

                return "Successful";
            }
            catch (Exception ex)
            {
                return "Fail" + ex.Message;
            }

        }

        public IActionResult SalaahCalendar(string Id)
        {
            Masjid Masjid = Repository
                            .Find<Masjid>(s => s.Id == Id)
                            .FirstOrDefault();

            cSalaahCalendar Model = new cSalaahCalendar(Masjid, _context);

            return View(Model);
        }

        public IActionResult PerpetualCalendar(string Id)
        {
            Masjid Masjid = Repository
                            .Find<Masjid>(s => s.Id == Id)
                            .FirstOrDefault();

            cPerpCalendar Model = new cPerpCalendar(Masjid);

            return View(Model);
        }

        public IActionResult PerpetualCalendarPrinter(string Id)
        {
            Masjid Masjid = Repository
                            .Find<Masjid>(s => s.Id == Id)
                            .FirstOrDefault();

            cPerpCalendar Model = new cPerpCalendar(Masjid);

            return View(Model);
        }

        public IActionResult PerpetualTimes(double Latitude, double Longitude, double TimeZone)
        {
            Masjid TempMasjid = new Masjid();
            TempMasjid.Latitude = Latitude;
            TempMasjid.Longitude = Longitude;

            TempMasjid.TimeZoneDiff = TimeZone;

            cPerpetualTime Time = new cPerpetualTime(DateTime.Now, TempMasjid, true);

            var _json = new
            {
                sehriEnds = Time.SehriEnds.ToString("HH:mm"),
                fajr = Time.Fajr.ToString("HH:mm"),
                sunrise = Time.Sunrise.ToString("HH:mm"),
                ishraaq = Time.Ishraaq.ToString("HH:mm"),
                zawaal = Time.Zawaal.ToString("HH:mm"),
                dhuhr = Time.Dhuhr.ToString("HH:mm"),
                asar1 = Time.AsrShafi.ToString("HH:mm"),
                asar2 = Time.AsrShafi.ToString("HH:mm"),
                sunset = Time.Sunset.ToString("HH:mm"),
                maghrib = Time.Maghrib.ToString("HH:mm"),
                isha = Time.Isha.ToString("HH:mm"),
            };

            return Json(_json);
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
            List<Masjid> Markers = Repository
                                    .GetAll<Masjid>()
                                    .Include(s => s.SalaahTimes)
                                    .ToList();

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

        public SalaahTime GetSalaahTime(Masjid Masjid, DateTime Val)
        {
            if (Masjid.SalaahTimesType == SalaahTimesType.ScheduleTime)
            {
                return Masjid.SalaahTimes
                            .Where(s => s.DayNumber <= Val.DayOfYear
                                    && s.Type == SalaahTimesType.ScheduleTime)
                            .OrderByDescending(x => x.DayNumber)
                            .FirstOrDefault();
            }
            else
            {
                return Masjid.SalaahTimes
                            .Where(s => s.DayNumber == Val.DayOfYear
                                    && s.TimeDate.Year <= Val.Year
                                    && s.Type == SalaahTimesType.DailyTime)
                            .OrderByDescending(x => x.TimeDate.Year)
                            .OrderByDescending(x => x.DayNumber)
                            .FirstOrDefault();
            }
        }

        public SalaahTime NextSalaahTime(Masjid Masjid, DateTime Val)
        {
            SalaahTime Time;

            if (Masjid.SalaahTimesType == SalaahTimesType.ScheduleTime)
            {
                Time = Masjid.SalaahTimes
                            .Where(s => s.DayNumber > Val.DayOfYear
                                    && s.Type == SalaahTimesType.ScheduleTime)
                            .OrderBy(x => x.DayNumber)
                            .FirstOrDefault();

                if (Time == null)
                {
                    Time = Masjid.SalaahTimes
                            .Where(s => s.Type == SalaahTimesType.ScheduleTime)
                            .OrderBy(x => x.DayNumber)
                            .FirstOrDefault();

                    if (Time != null)
                    {
                        Time.TimeDate = new DateTime(Val.Year + 1, 1, 1);
                        Time.TimeDate = Time.TimeDate.AddDays(Time.DayNumber - 1);
                    }
                }
            }
            else
            {
                Time = Masjid.SalaahTimes
                            .Where(s => s.DayNumber > Val.DayOfYear
                                    && s.Type == SalaahTimesType.DailyTime
                                    && s.TimeDate.Year <= Val.Year
                                    && s.IsATimeChange == true)
                            .OrderByDescending(s => s.TimeDate.Year)
                            .OrderBy(x => x.DayNumber)
                            .FirstOrDefault();

                if (Time == null)
                {
                    Time = Masjid.SalaahTimes
                            .Where(s => s.Type == SalaahTimesType.DailyTime
                                    && s.TimeDate.Year <= Val.Year + 1
                                    && s.IsATimeChange == true)
                            .OrderByDescending(s => s.TimeDate.Year)
                            .OrderBy(x => x.DayNumber)
                            .FirstOrDefault();
                }
            }

            return Time;
        }

        public Masjid GetCountDown(Masjid Masjid)
        {
            MasjidCountDown CountDown = new MasjidCountDown();

            SalaahTime Times = GetSalaahTime(Masjid, DateTime.Now);

            DateTime Now = DateTime.Now;

            cPerpetualTime PepTime = new cPerpetualTime(DateTime.Now, Masjid, false);

            if (Now.TimeOfDay < Times.FajrAdhaan.TimeOfDay)
            {
                CountDown.NextSalaah = "Fajr Adhaan";
                CountDown.CountDown = TimeDiff(Times.FajrAdhaan);
                CountDown.SalaahTime = Times.FajrAdhaan.ToString("HH:mm");
            }
            else if (Now.TimeOfDay < Times.FajrSalaah.TimeOfDay)
            {
                CountDown.NextSalaah = "Fajr Salaah";
                CountDown.CountDown = TimeDiff(Times.FajrSalaah);
                CountDown.SalaahTime = Times.FajrSalaah.ToString("HH:mm");
            }

            // JUMMAH//
            else if (Now.TimeOfDay < Times.JumuahAdhaan.TimeOfDay
                    && Masjid.JummahFacility
                    && Now.DayOfWeek == DayOfWeek.Friday)
            {
                CountDown.NextSalaah = "Jumuah Adhaan";
                CountDown.CountDown = TimeDiff(Times.JumuahAdhaan);
                CountDown.SalaahTime = Times.JumuahAdhaan.ToString("HH:mm");
            }
            else if (Now.TimeOfDay < Times.JumuahSalaah.TimeOfDay
                    && Masjid.JummahFacility
                    && Now.DayOfWeek == DayOfWeek.Friday)
            {
                CountDown.NextSalaah = "Jumuah Salaah";
                CountDown.CountDown = TimeDiff(Times.JumuahSalaah);
                CountDown.SalaahTime = Times.JumuahSalaah.ToString("HH:mm");
            }

            //DHUHR//
            else if (Now.TimeOfDay < Times.DhuhrAdhaan.TimeOfDay)
            {
                CountDown.NextSalaah = "Dhuhr Adhaan";
                CountDown.CountDown = TimeDiff(Times.DhuhrAdhaan);
                CountDown.SalaahTime = Times.DhuhrAdhaan.ToString("HH:mm");
            }
            else if (Now.TimeOfDay < Times.DhuhrSalaah.TimeOfDay)
            {
                CountDown.NextSalaah = "Dhuhr Salaah";
                CountDown.CountDown = TimeDiff(Times.DhuhrSalaah);
                CountDown.SalaahTime = Times.DhuhrSalaah.ToString("HH:mm");
            }

            else if (Now.TimeOfDay < Times.AsrAdhaan.TimeOfDay)
            {
                CountDown.NextSalaah = "Asr Adhaan";
                CountDown.CountDown = TimeDiff(Times.AsrAdhaan);
                CountDown.SalaahTime = Times.AsrAdhaan.ToString("HH:mm");
            }
            else if (Now.TimeOfDay < Times.AsrSalaah.TimeOfDay)
            {
                CountDown.NextSalaah = "Asr Salaah";
                CountDown.CountDown = TimeDiff(Times.AsrSalaah);
                CountDown.SalaahTime = Times.AsrSalaah.ToString("HH:mm");
            }

            else if (Now.TimeOfDay < PepTime.Maghrib.TimeOfDay)
            {
                CountDown.NextSalaah = "Maghrib";
                CountDown.CountDown = TimeDiff(PepTime.Maghrib);
                CountDown.SalaahTime = PepTime.Maghrib.ToString("HH:mm");
            }

            else if (Now.TimeOfDay < Times.IshaAdhaan.TimeOfDay)
            {
                CountDown.NextSalaah = "Isha Adhaan";
                CountDown.CountDown = TimeDiff(Times.IshaAdhaan);
                CountDown.SalaahTime = Times.IshaAdhaan.ToString("HH:mm");
            }
            else if (Now.TimeOfDay < Times.IshaSalaah.TimeOfDay)
            {
                CountDown.NextSalaah = "Isha Salaah";
                CountDown.CountDown = TimeDiff(Times.IshaSalaah);
                CountDown.SalaahTime = Times.IshaSalaah.ToString("HH:mm");
            }
            else
            {
                //Check Next Days Fajr
                SalaahTime TomorrowsTime = GetSalaahTime(Masjid, DateTime.Now.AddDays(1));
                CountDown.NextSalaah = "Fajr Adhaan";
                CountDown.CountDown = "N/A";// TimeDiff(TomorrowsTime.FajrAdhaan);
                CountDown.SalaahTime = TomorrowsTime.FajrAdhaan.ToString("HH:mm");
            }

            Masjid.CountDown = CountDown;

            return Masjid;
        }

        public string TimeDiff(DateTime Val)
        {
            TimeSpan Diff = Val.Subtract(DateTime.Now);

            if (Diff.Minutes < 10)
                return Diff.Hours.ToString() + ":0" + Diff.Minutes.ToString();
            else
                return Diff.Hours.ToString() + ":" + Diff.Minutes.ToString();
        }

        public IActionResult NearestMasjidsTable(double Latitude, double Longitude, int Radius)
        {
            try
            {
                List<Masjid> NearestM = NearestMasjids(Latitude, Longitude, Radius);

                foreach(var M in NearestM)
                {
                    if (M.SalaahTimes.Count > 0)
                        M.CountDown = GetCountDown(M).CountDown;
                }

                var _json = NearestM.Select(u => new
                {
                    Id = u.Id,
                    Masjid = u.Name + ", " + u.Town + ", " + u.Country,
                    Distance = u.Distance + " KM",
                    NextSalaah = (u.CountDown != null) ? u.CountDown.NextSalaah : "",
                    Countdown = (u.CountDown != null) ? u.CountDown.CountDown : "",
                    SalaahTime = (u.CountDown != null) ? u.CountDown.SalaahTime : "",
                    LadiesFacility = (u.LadiesFacility) ? "Yes" : "No",
                })
                .ToList();

                return Json(new { data = _json }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                return Json(new { data = "" }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }
    }
}
