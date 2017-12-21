using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taqweem.Models.ManageViewModels
{
    public class FileViewModel
    {
        [DataType(DataType.Upload)]
        public IFormFile MyFile { get; set; }
    }
}
