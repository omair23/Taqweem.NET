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

            FileViewModel Model = new FileViewModel();

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
    }
}
