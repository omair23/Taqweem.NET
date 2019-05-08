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
        public IEnumerable<object> GetMasjids()
        {
            return _taqweemService.MasjidGetAll()
                                    .Select(d => new MasjidDTOLight
                                    {
                                        Id = d.Id,
                                        Name = d.Name,
                                        Town = d.Town,
                                        Country = d.Country
                                    })
                                   .ToList();
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

        [HttpGet("{id}", Name = "GetMasjid")]
        public Masjid GetMasjid(string Id)
        {
            var item = _taqweemService.MasjidGetById(Id);

            return item;
        }

    }
}