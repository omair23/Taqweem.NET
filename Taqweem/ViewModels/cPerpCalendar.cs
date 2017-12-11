using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taqweem.ViewModels
{
    public class cPerpCalendar
    {
        public List<DateTime> Months;

        public List<cPerpetualTime> Times;

        public cPerpCalendar()
        {
            Months = new List<DateTime>();

            for(int i=1; i <=12; i++)
            {
                Months.Add(new DateTime(DateTime.Now.Year, i, 1));
            }

            Times = new List<cPerpetualTime>();

            foreach(var Item in Months)
            {
                int Days = DateTime.DaysInMonth(DateTime.Now.Year, Item.Month);

                for (int i=1; i<= Days; i++)
                {
                    Times.Add(new cPerpetualTime(new DateTime(DateTime.Now.Year, Item.Month, i)));
                }

            }
        }

    }
}
