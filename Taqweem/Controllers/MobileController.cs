using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
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

        [Microsoft.AspNetCore.Mvc.HttpGet("{id}", Name = "GetMasjid")]
        public ActionResult<Masjid> GetMasjid(string Id)
        {
            var item = _taqweemService.MasjidGetById(Id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        //[ResponseType(typeof(Masjid))]
        //public async Task<IHttpActionResult> MasjidGetInformation(string Id)
        //{

        //    return Ok(new object());
        //    //var book = await (from b in db.Books.Include(b => b.Author)
        //    //                  where b.BookId == id
        //    //                  select new object
        //    //                  {
        //    //                      Title = b.Title,
        //    //                      Genre = b.Genre,
        //    //                      PublishDate = b.PublishDate,
        //    //                      Price = b.Price,
        //    //                      Description = b.Description,
        //    //                      Author = b.Author.Name
        //    //                  }).FirstOrDefaultAsync();

        //    //if (book == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //return Ok(book);
        //}


    }
}