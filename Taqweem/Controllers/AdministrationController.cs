using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Taqweem.Models;
using Taqweem.Models.ManageViewModels;
using Taqweem.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;
using Taqweem.Data;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.ViewModels;
using Microsoft.EntityFrameworkCore;
using Taqweem.ViewModels.AdminViewModels;
using System.Web;

namespace Taqweem.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly ApplicationDbContext _context;
        private readonly EFRepository Repository;

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public AdministrationController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<AdministrationController> logger,
          UrlEncoder urlEncoder,
          ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _context = context;
            Repository = new EFRepository(_context);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpPost]
        public string AlignTimeZones(string Id)
        {
            if (!IsSuperUser())
            {
                return "Unauthorised";
            }

            try
            {
                Dictionary<string, string> CountryTimeZones = new Dictionary<string, string>();

                CountryTimeZones.Add("South Africa", "South Africa Standard Time");
                CountryTimeZones.Add("Saudi Arabia", "Arab Standard Time");
                CountryTimeZones.Add("Iraq", "Arabic Standard Time");
                CountryTimeZones.Add("Jordan", "E. Europe Standard Time");
                CountryTimeZones.Add("Kuwait", "Arab Standard Time");
                CountryTimeZones.Add("Tanzania", "E. Africa Standard Time");
                CountryTimeZones.Add("United Arab Emirates", "Arabian Standard Time");
                CountryTimeZones.Add("Botswana", "South Africa Standard Time");
                CountryTimeZones.Add("Namibia", "Namibia Standard Time");
                CountryTimeZones.Add("Oman", "Arabian Standard Time");
                CountryTimeZones.Add("India", "India Standard Time");
                CountryTimeZones.Add("Myanmar (Burma)", "Myanmar Standard Time");
                CountryTimeZones.Add("Uzbekistan", "West Asia Standard Time");
                CountryTimeZones.Add("Turkey", "Turkey Standard Time");
                CountryTimeZones.Add("Angola", "W. Central Africa Standard Time");
                CountryTimeZones.Add("Democratic Republic of Congo", "W. Central Africa Standard Time");
                CountryTimeZones.Add("Bahrain", "Arab Standard Time");
                CountryTimeZones.Add("Qatar", "Arab Standard Time");
                CountryTimeZones.Add("United Kingdom", "GMT Standard Time");
                CountryTimeZones.Add("Malawi", "South Africa Standard Time");

                List<Masjid> Masjids = Repository.GetAll<Masjid>().ToList();

                foreach(Masjid masjid in Masjids)
                {
                    if (CountryTimeZones.ContainsKey(masjid.Country))
                    {
                        masjid.TimeZoneId = CountryTimeZones[masjid.Country];
                    }
                    else
                    {
                        masjid.TimeZoneId = "UTC";
                    }
                }

                Repository.UpdateMultiple(Masjids);

                return "Successful";
            }
            catch (Exception ex)
            {
                return "Failed " + ex.Message;
            }
        }

        //public string ConfirmationEmail()
        //{
        //    if (!IsSuperUser())
        //    {
        //        return "Unauthorised";
        //    }

        //    try
        //    {
        //        //List<ApplicationUser> Users = Repository.GetAll<ApplicationUser>().ToList();//.Take(5).ToList();

        //        List<ApplicationUser> Users = Repository.Find<ApplicationUser>(s => s.Id == "BB3143F3-AE44-CB62-A24D-A78304E1D1B4").ToList();

        //        foreach (ApplicationUser User in Users)
        //        {
        //            var code = _userManager.GenerateEmailConfirmationTokenAsync(User).Result;

        //            code = HttpUtility.UrlEncode(code);

        //            var callbackUrl = Url.EmailConfirmationLink(User.Id, code, Request.Scheme);
        //            //var sendemail = _emailSender.SendEmailConfirmationAsync(User.Email, callbackUrl);

        //            string Content = "<p>Dear Taqweem User</p><br><p>Thank you for signing up as a Masjid Administrator.</p><br>" +
        //                     "<p>Your details have been submitted successfully. There is only one step left: to activate your account.</p><br>" +
        //                      $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>" +
        //                      "<br><br><p>Shukran</p><p><strong>Taqweem Team</strong></p>";


        //            var send = _emailSender.SendEmailAsync(User.Email, "Taqweem Account Confirmation", Content);
        //            //var send = _emailSender.SendEmailAsync("omair334@gmail.com", "Taqweem Account Confirmation", Content);
        //        }

        //        //return send.Status.ToString();

        //        return "Successful";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Failed " + ex.Message;
        //    }
        //}

        //public string WelcomeEmail()
        //{
        //    if (!IsSuperUser())
        //    {
        //        return "Unauthorised";
        //    }

        //    try
        //    {
        //        List<ApplicationUser> Users = Repository.GetAll<ApplicationUser>().ToList();

        //        //TO DO Update Links to main site
        //        string Link = "http://taqweem.rapidsoft.co.za/";

        //        string Text = "<p>As Salaamu Alaikum from the Taqweem team</p><p>We are proud to announce the go-live of our <strong>new and improved website</strong> which can be visited on the new link shown below:</p><p>" +
        //            "<a href='" + Link + "'>" +
        //            "http://taqweem.rapidsoft.co.za/</a></p><p>We will send out an account verification email following this email in order to activate your account. Once activated, please log on to the Taqweem dashboard using your <strong>existing email address</strong> and the following password: <strong><u>Masjid@1</u></strong></p><p>Following the confirmation of your account, we strongly advise changing your password. This can be done by simply logging in and clicking the Password option on the menu bar.</p><p>At Taqweem, our aim is to provide extensive coverage of Masaajid and Salaah Times across the globe. Therefore we require your help, we are crowd-sourced website. Here are some tips and suggestions for you, the user, to help improve and expand Taqweem:</p><ul><li>Adding new Masjids to our system</li><li>Linking Masajid’s “Website” tags on Google Maps to the relevant masjid page on Taqweem</li><li>Encouraging people to register as Administrators of Masajid in order to keep the information and salaah times up-to-date</li></ul><p>Should you require any assistance with using the site, please contact the team on the “About” page.</p><p>We urge you to spread the word, send us your feedback and contribute towards maintaining the accuracy and completeness of information on the site.</p><p>Shukran</p><p><strong>Taqweem Team</strong></p>";

        //        foreach (ApplicationUser User in Users)
        //        {
        //            var stat = _emailSender.SendEmailAsync(User.Email, "Taqweem Version 2.0", Text).Status;
        //        }

        //        return "Successful";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Failed " + ex.Message;
        //    }
        //}

        public bool IsSuperUser()
        {
            ApplicationUser user = GetCurrentUserAsync().Result;

            var userId = user?.Id;

            if (user == null | userId == null)
            {
                return false;
            }

            if(user.IsSuperUser == false)
            {
                return false;
            }

            return true;
        }

        public IActionResult Index()
        {
            if (!IsSuperUser())
            {
                return RedirectToAction("Index", "Manage");
            }

            AdminDashboardViewModel Model = new AdminDashboardViewModel();

            try
            {
                Model.Users = Repository.GetAll<ApplicationUser>().Count();
                Model.Masjids = Repository.GetAll<Masjid>().Count();
                Model.MasjidSalaahTimes = Repository.GetAll<SalaahTime>().Select(s => s.MasjidId).Distinct().Count();
                Model.MasjidNotices = Repository.GetAll<Notice>().Select(s => s.MasjidId).Distinct().Count();
            }
            catch(Exception ex)
            {

            }

            return View(Model);
        }

        [HttpPost]
        public string UploadUsers(IFormFile file)
        {
            try
            {
                if (!IsSuperUser())
                {
                    return "Failed";
                }

                if (ModelState.IsValid)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        file.CopyTo(stream);

                        byte[] byteArray = stream.ToArray();

                        Stream stream2 = new MemoryStream(byteArray);

                        var package = new ExcelPackage(stream2);

                        var z = package.Workbook.Worksheets[1];

                        ExcelWorksheet Sheet = package.Workbook.Worksheets[1];

                        for (int i = 2; i <= Sheet.Dimension.End.Row; i++)
                        {
                            try
                            {
                                double OldMasjidId1 = (double)Sheet.Cells[i, 2].Value;

                                int OldMasjidId = Convert.ToInt32(Math.Round(OldMasjidId1, 0));

                                var NewMasjid = Repository.Find<Masjid>(s => s.OldSiteId == OldMasjidId).FirstOrDefault();

                                var user = new ApplicationUser
                                {
                                    UserName = Sheet.Cells[i, 1].Value.ToString(),
                                    Email = Sheet.Cells[i, 1].Value.ToString(),
                                    FullName = "TBC",
                                    EmailConfirmed = false,
                                    ActiveStatus = UserStatus.Active,
                                    MasjidId = NewMasjid.Id,
                                };

                                user.Id = Sheet.Cells[i, 7].Value.ToString();

                                try
                                {
                                    user.CreatedAt = DateTime.FromOADate((double)Sheet.Cells[i, 5].Value);
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        user.CreatedAt = DateTime.ParseExact(Sheet.Cells[i, 5].Value.ToString(),
                                            "yyyy/MM/dd hh:mm:ss tt",
                                            System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception ex2)
                                    {

                                    }
                                }


                                var Password = "Masjid@1"; //Sheet.Cells[i, 1].Value.ToString().ToUpper();

                                var result = _userManager.CreateAsync(user, Password).Result;

                                if (!result.Succeeded)
                                    throw new Exception();
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        package.Dispose();
                    }

                    return "Successful";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return "Fail" + ex.Message;
            }
        }

        [HttpPost]
        public string UploadMasjid(IFormFile file)
        {
            try
            {
                if (!IsSuperUser())
                {
                    return "Failed";
                }

                if (ModelState.IsValid)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        file.CopyTo(stream);

                        byte[] byteArray = stream.ToArray();

                        Stream stream2 = new MemoryStream(byteArray);

                        var package = new ExcelPackage(stream2);

                        var z = package.Workbook.Worksheets[1];

                        ExcelWorksheet Sheet = package.Workbook.Worksheets[1];

                        List<Masjid> Masjids = new List<Masjid>();

                        for (int i = 2; i <= Sheet.Dimension.End.Row; i++)
                        {
                            try
                            {
                                var M = new Masjid();

                                M.OldSiteId = Convert.ToInt32(Sheet.Cells[i, 1].Value.ToString());

                                if (M.OldSiteId == 1)
                                    continue;

                                M.Name = Sheet.Cells[i, 2].Value.ToString();
                                M.Town = Sheet.Cells[i, 3].Value.ToString();
                                M.Country = Sheet.Cells[i, 4].Value.ToString();

                                M.Latitude = Convert.ToDouble(Sheet.Cells[i, 5].Value.ToString());
                                M.Longitude = Convert.ToDouble(Sheet.Cells[i, 6].Value.ToString());

                                //M.JuristMethod = (JuristicMethod)Enum.ToObject(typeof(JuristicMethod), (int)Sheet.Cells[i, 9].Value);

                                string Ladies = Sheet.Cells[i, 10].Value.ToString();
                                if (Ladies == "Yes")
                                    M.LadiesFacility = true;
                                else
                                    M.LadiesFacility = false;

                                double Jummah = (double)Sheet.Cells[i, 11].Value;
                                if (Jummah == 1)
                                    M.JummahFacility = true;
                                else
                                    M.JummahFacility = false;

                                M.Address = Sheet.Cells[i, 12].Value == null ? "" : Sheet.Cells[i, 12].Value.ToString();

                                M.Contact = Sheet.Cells[i, 13].Value == null ? "" : Sheet.Cells[i, 13].Value.ToString();

                                M.GeneralInfo = Sheet.Cells[i, 14].Value == null ? "" : Sheet.Cells[i, 14].Value.ToString();

                                M.TimeZoneId = "South Africa Standard Time";

                                Masjids.Add(M);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        Masjids = Masjids.OrderBy(s => s.OldSiteId).ToList();

                        Repository.AddMultiple(Masjids);

                        package.Dispose();
                    }

                    return "Successful";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return "Fail" + ex.Message;
            }
        }

        [HttpPost]
        public string UploadCurrencies(IFormFile file)
        {
            try
            {
                if (!IsSuperUser())
                {
                    return "Failed";
                }

                if (ModelState.IsValid)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        file.CopyTo(stream);

                        byte[] byteArray = stream.ToArray();

                        Stream stream2 = new MemoryStream(byteArray);

                        var package = new ExcelPackage(stream2);

                        var z = package.Workbook.Worksheets[1];

                        ExcelWorksheet Sheet = package.Workbook.Worksheets[1];

                        List<Currency> Currencies = new List<Currency>();

                        for (int i = 2; i <= Sheet.Dimension.End.Row; i++)
                        {
                            try
                            {
                                var M = new Currency();

                                M.Code = Sheet.Cells[i, 1].Value.ToString();
                                M.Name = Sheet.Cells[i, 2].Value.ToString();
                                M.Locations = Sheet.Cells[i, 3].Value.ToString();

                                M.ConversionRate = Convert.ToDouble(Sheet.Cells[i, 4].Value.ToString());

                                M.Symbol = Sheet.Cells[i, 5].Value.ToString();

                                M.FractionalUnit = Sheet.Cells[i, 6].Value.ToString();

                                M.NumberToBasic = Convert.ToInt32(Sheet.Cells[i, 7].Value.ToString());

                                M.Flag = Sheet.Cells[i, 8].Value.ToString();

                                Currencies.Add(M);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                        Repository.AddMultiple(Currencies);

                        package.Dispose();
                    }

                    return "Successful";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return "Fail" + ex.Message;
            }
        }

        public IActionResult Masjids()
        {
            if (!IsSuperUser())
            {
                return RedirectToAction("Index", "Manage");
            }

            List<Masjid> AllMasjids = Repository.GetAll<Masjid>().OrderByDescending(s => s.CreatedAt).ToList();

            MasjidListViewModel Model = new MasjidListViewModel();

            Model.Masjids = AllMasjids;

            return View(Model);
        }

        public IActionResult Users()
        {
            if (!IsSuperUser())
            {
                return RedirectToAction("Index", "Manage");
            }

            List<ApplicationUser> AllUsers = Repository
                                                .GetAll<ApplicationUser>()
                                                .OrderByDescending(s => s.CreatedAt)
                                                .Include(s => s.Masjid)
                                                .ToList();

            UserListViewModel Model = new UserListViewModel();

            Model.Users = AllUsers;

            return View(Model);
        }

        [HttpPost]
        public string MasjidDelete(string Id)
        {
            if (!IsSuperUser())
            {
                return "Unauthorised";
            }

            try
            {
                List<Notice> Notices = Repository.Find<Notice>(s => s.MasjidId == Id).ToList();

                List<SalaahTime> SalaahTimes = Repository.Find<SalaahTime>(s => s.MasjidId == Id).ToList();

                Masjid Masjid = Repository.Find<Masjid>(s => s.Id == Id).FirstOrDefault();

                Repository.DeleteMultiple(Notices);

                Repository.DeleteMultiple(SalaahTimes);

                Repository.Delete(Masjid);

                return "Successful"; 
            }
            catch (Exception ex)
            {
                return "Failed " + ex.Message;
            }
        }

        [HttpPost]
        public string UserActivate(string Id)
        {
            if (!IsSuperUser())
            {
                return "Unauthorised";
            }

            try
            {
                ApplicationUser user = Repository.Find<ApplicationUser>(s => s.Id == Id).FirstOrDefault();

                user.ActiveStatus = UserStatus.Active;

                Repository.Update(user);

                return "Successful";
            }
            catch (Exception ex)
            {
                return "Failed " + ex.Message;
            }
        }

        [HttpPost]
        public string UserDeactivate(string Id)
        {
            if (!IsSuperUser())
            {
                return "Unauthorised";
            }

            try
            {
                ApplicationUser user = Repository.Find<ApplicationUser>(s => s.Id == Id).FirstOrDefault();

                user.ActiveStatus = UserStatus.Suspended;

                Repository.Update(user);

                return "Successful";
            }
            catch (Exception ex)
            {
                return "Failed " + ex.Message;
            }
        }
    }
}
