using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taqweem.Data;
using Taqweem.Models;

namespace Taqweem.Services
{
    public class TaqweemService
    {
        private readonly EFRepository Repository;
        private readonly ApplicationDbContext _context;

        public TaqweemService(ApplicationDbContext context)
        {
            _context = context;
            Repository = new EFRepository(_context);
        }

        public Masjid MasjidGetById(string Id)
        {
            return Repository.GetByID<Masjid>(s => s.Id == Id);
        }
    }
}
