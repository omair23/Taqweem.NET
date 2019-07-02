using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Taqweem.Classes;
using Taqweem.Models;
using Taqweem.Services;
using TimeZoneConverter;

namespace Taqweem.Controllers
{
    public class MobileController : Controller
    {
        public readonly TaqweemService _taqweemService;

        public MobileController(TaqweemService taqweemService)
        {
            _taqweemService = taqweemService;
        }

        [HttpGet]
        public async Task<IEnumerable<object>> GetMasjids()
        {
            return _taqweemService.MasjidGetAll()
                                    .Select(d => new MasjidDTOLight
                                    {
                                        Id = d.Id,
                                        Name = d.Name,
                                        Town = d.Town,
                                        Country = d.Country
                                    })
                                    .Take(10)
                                   .ToList();
        }

        [HttpGet]
        public async Task<object> GetMasjidById(string id)
        {
            var d = await _taqweemService
                                    .MasjidGetByIdAsync(id);

            MasjidDTO returnObject = new MasjidDTO
            {
                Id = d.Id,
                Name = d.Name,
                Town = d.Town,
                Country = d.Country,
                LastUpdate = d.LastUpdate,
                Latitude = d.Latitude,
                Longitude = d.Longitude,
                Height = d.Height,
                TimeZoneId = d.TimeZoneId,
                JuristMethod = d.JuristMethod,
                LadiesFacility = d.LadiesFacility,
                JummahFacility = d.JummahFacility,
                Contact = d.Contact,
                Address = d.Address,
                GeneralInfo = d.GeneralInfo,
                MaghribAdhaanDelay = d.MaghribAdhaanDelay,
                SalaahTimesType = d.SalaahTimesType,
                IsSpecialDayEnabled = d.IsSpecialDayEnabled,
                IsPublicHolidaySpecialTimesEnabled = d.IsPublicHolidaySpecialTimesEnabled,
                SpecialDayNumber = d.SpecialDayNumber,
                TZTimeZone = TZConvert.WindowsToIana(d.TimeZoneId)
        };

            return returnObject;
        }

        [HttpGet]
        public async Task<object> GetMasjidSalaahTimesById(string id)
        {
            try
            {
                Masjid masjid = await _taqweemService.MasjidGetByIdIncludedAsync(id);

                SalaahTime times = _taqweemService.GetSalaahTime(masjid, DateTime.Now);

                if (times != null)
                {

                    SalaahTimeDTO returnObject = new SalaahTimeDTO
                    {
                        MasjidId = times.MasjidId,
                        Type = times.Type,
                        IsATimeChange = times.IsATimeChange,
                        TimeDate = times.TimeDate,
                        DayNumber = times.DayNumber,
                        FajrAdhaan = times.FajrAdhaan,
                        FajrSalaah = times.FajrSalaah,
                        DhuhrAdhaan = times.DhuhrAdhaan,
                        DhuhrSalaah = times.DhuhrSalaah,
                        JumuahAdhaan = times.JumuahAdhaan,
                        JumuahSalaah = times.JumuahSalaah,
                        SpecialDhuhrAdhaan = times.SpecialDhuhrAdhaan,
                        SpecialDhuhrSalaah = times.SpecialDhuhrSalaah,
                        AsrAdhaan = times.AsrAdhaan,
                        AsrSalaah = times.AsrSalaah,
                        IshaAdhaan = times.IshaAdhaan,
                        IshaSalaah = times.IshaSalaah,
                    };

                    return returnObject;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        public IEnumerable<object> GetMasjidsByTerm(string Term)
        {
            Term = Term != null ? Term : "";

            return _taqweemService.MasjidGetByTerm(Term)
                                    .Select(d => new MasjidDTOLight
                                    {
                                        Id = d.Id,
                                        Name = d.Name,
                                        Town = d.Town,
                                        Country = d.Country
                                    })
                                   .ToList();
        }

    }
}