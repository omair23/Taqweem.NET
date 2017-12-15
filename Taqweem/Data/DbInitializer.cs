using System.Linq;
using Taqweem.Models;

namespace Taqweem.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Masjids.Any())
            {
                return;   // DB has been seeded
            }

            Masjid s = new Masjid();
            s.Name = "Masjid Muaadh bin Jabal Crosby";
            s.Town = "Johannesburg";
            s.Country = "South Africa";
            s.Latitude = -26.195149;
            s.Longitude = 27.990238;
            s.TimeZone = 2;

            context.Masjids.Add(s);
            context.SaveChanges();
        }
    }
}
