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

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            Repository = new EFRepository(_context);
            _emailSender = emailSender;
        }

        public void DBInit()
        {
            List<Models.TimeZone> TimeZones = Repository.GetAll<Models.TimeZone>().ToList();

            if(TimeZones.Count < 1)
            {
                foreach(var Zone in TimeZoneInfo.GetSystemTimeZones())
                {
                    Models.TimeZone T = new Models.TimeZone();
                    T.Id = Zone.Id;
                    T.DaylightName = Zone.DaylightName;
                    T.DisplayName = Zone.DisplayName;
                    T.StandardName = Zone.StandardName;
                    T.SupportsDaylightSavingTime = Zone.SupportsDaylightSavingTime;

                    T.DefaultUTCDifference = cCalculations.GetTimeZoneDifference(T.Id, DateTime.Now);

                    TimeZones.Add(T);
                }

                Repository.AddMultiple(TimeZones);
            }

            List<Masjid> AllMasjids = Repository.GetAll<Masjid>().ToList();

            //DB INIT
            if (AllMasjids.Count < 1)
            {
                Masjid s = new Masjid();
                s.Id = "5f3e7169-ab20-4b34-bb27-2e86eefee2c1";
                s.Name = "Masjid Muaadh bin Jabal - Crosby";
                s.Town = "Johannesburg";
                s.Country = "South Africa";
                s.Latitude = -26.195149;
                s.Longitude = 27.990238;
                s.OldSiteId = 1;
                s.TimeZoneId = "South Africa Standard Time";

                Repository.Add(s);
            }
            ////

            List<ApplicationUser> Users = Repository.GetAll<ApplicationUser>().ToList();

            if (Users.Count < 1)
            {
                var OmairEmail = "omair334@gmail.com";

                var user = new ApplicationUser {
                                                UserName = OmairEmail,
                                                Email = OmairEmail,
                                                Id = "513f1fe1-8e01-4c62-b332-ee8a0f7e2c29",
                                                FullName = "Omair Kazi",
                                                EmailConfirmed = true,
                                                ActiveStatus = UserStatus.Active,
                                                IsSuperUser = true,
                                                MasjidId = "5f3e7169-ab20-4b34-bb27-2e86eefee2c1"};

                var Password = "Open@1";

                var result = _userManager.CreateAsync(user, Password).Result;
            }
        }

        public IActionResult Index()
        {
            DBInit();

            List<Masjid> AllMasjids = Repository.GetAll<Masjid>().ToList();

            Markers Model = new Markers();

            Model.Marker = AllMasjids;

            return View(Model);

            //EmailModel Model = new EmailModel();

            //Model.Content = "TEXT";

            //return View("EmailTemplate", Model);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult MasjidList()
        {
            List<Masjid> AllMasjids = Repository.GetAll<Masjid>().OrderBy(s => s.OldSiteId).ToList();

            MasjidListViewModel Model = new MasjidListViewModel();

            Model.Masjids = AllMasjids;

            return View(Model);
        }

        public IActionResult MasjidInfo(string Id)
        {
            Masjid Info = Repository
                            .Find<Masjid>(s => s.Id == Id)
                            .Include(s => s.SalaahTimes)
                            .Include(s => s.TimeZone)
                            .FirstOrDefault();

            MasjidInfoViewModel Model = new MasjidInfoViewModel(Info);

            Model.Markers.Marker = Repository.GetAll<Masjid>().ToList();

            Model.Users = Repository
                            .Find<ApplicationUser>(s => s.MasjidId == Id)
                            .ToList();

            Model.Notices = Repository
                                    .Find<Notice>(s => s.MasjidId == Id)
                                    .ToList();

            Model.SalaahTime = GetSalaahTime(Info, DateTime.Now);

            //if (Model.Masjid.SalaahTimesType == SalaahTimesType.ScheduleTime)
            //{
            //    Model.SalaahTime = Repository
            //                .Find<SalaahTime>(s => s.MasjidId == Id
            //                && s.DayNumber <= DateTime.Now.DayOfYear)
            //                .OrderByDescending(x => x.DayNumber)
            //                .FirstOrDefault();

            //    Model.NextSalaahTime = Repository
            //                    .Find<SalaahTime>(s => s.MasjidId == Id
            //                    && s.DayNumber > DateTime.Now.DayOfYear)
            //                    .OrderBy(x => x.DayNumber)
            //                    .FirstOrDefault();

            //    if (Model.NextSalaahTime != null)
            //    {
            //        DateTime NextDate = new DateTime(DateTime.Now.Year, 1, 1);
            //        NextDate = NextDate.AddDays(Model.NextSalaahTime.DayNumber - 1);

            //        Model.NextPerpetualTime = new cPerpetualTime(NextDate, Info);
            //    }
            //}
            //else
            //{
            //    Model.SalaahTime = Repository
            //                .Find<SalaahTime>(s => s.MasjidId == Id
            //                && s.DayNumber == DateTime.Now.DayOfYear)
            //                .FirstOrDefault();

            //    //TO DO Implement Calculation for next salaah time for huge list

            //    Model.NextSalaahTime = Repository
            //                    .Find<SalaahTime>(s => s.MasjidId == Id
            //                    && s.DayNumber > DateTime.Now.DayOfYear)
            //                    .OrderBy(x => x.DayNumber)
            //                    .FirstOrDefault();

            //    if (Model.NextSalaahTime != null)
            //    {
            //        DateTime NextDate = new DateTime(DateTime.Now.Year, 1, 1);
            //        NextDate = NextDate.AddDays(Model.NextSalaahTime.DayNumber - 1);

            //        Model.NextPerpetualTime = new cPerpetualTime(NextDate, Info);
            //    }
            //}

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

                Repository.Add(Masjid);

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

                _emailSender.SendEmailAsync(CVM.Email, "Taqweem Feedback", Message);

                return "Successful";
            }
            catch (Exception ex)
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

            cPerpetualTime Time = new cPerpetualTime(DateTime.Now, TempMasjid);

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
                            .Where(s => s.DayNumber <= Val.DayOfYear)
                            .OrderByDescending(x => x.DayNumber)
                            .FirstOrDefault();
            }
            else
            {
                return Masjid.SalaahTimes
                            .Where(s => s.DayNumber == Val.DayOfYear)
                            .FirstOrDefault();
            }
        }

        public Masjid GetCountDown(Masjid Masjid)
        {
            MasjidCountDown CountDown = new MasjidCountDown();

            SalaahTime Times = GetSalaahTime(Masjid, DateTime.Now);

            DateTime Now = DateTime.Now;

            cPerpetualTime PepTime = new cPerpetualTime(DateTime.Now, Masjid);

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
