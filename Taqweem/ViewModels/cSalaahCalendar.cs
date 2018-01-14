using System;
using System.Collections.Generic;
using System.Linq;
using Taqweem.Data;
using Taqweem.Models;

namespace Taqweem.ViewModels
{
    public class cSalaahCalendar
    {
        private readonly ApplicationDbContext _context;
        private readonly EFRepository Repository;

        public Masjid Masjid;

        public List<DateTime> Months;

        public List<SalaahTime> Times;

        public SalaahTimesType Type;

        public cSalaahCalendar(Masjid pMasjid, ApplicationDbContext context)
        {
            _context = context;
            Repository = new EFRepository(_context);

            Masjid = pMasjid;

            Months = new List<DateTime>();

            for(int i=1; i <=12; i++)
            {
                Months.Add(new DateTime(DateTime.Now.Year, i, 1));
            }

            Times = new List<SalaahTime>();

            if (Masjid.SalaahTimesType == SalaahTimesType.DailyTime)
            {
                Times = Repository
                        .Find<SalaahTime>(s => s.MasjidId == Masjid.Id
                                            && s.TimeDate.Year == DateTime.Now.Year
                                            && s.Type == SalaahTimesType.DailyTime)
                        .OrderBy(s => s.DayNumber)
                        .ToList();

                Type = SalaahTimesType.DailyTime;
            }
            else
            {
                Times = Repository
                                        .Find<SalaahTime>(s => s.MasjidId == Masjid.Id
                                                            && s.Type == SalaahTimesType.ScheduleTime)
                                        .OrderBy(s => s.DayNumber)
                                        .ToList();

                Type = SalaahTimesType.ScheduleTime;
            }
        }

    }
}
