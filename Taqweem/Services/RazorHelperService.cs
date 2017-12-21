using System.Collections.Generic;

namespace Taqweem.Services
{
    public class SelectList
    {
        public string Value { get; set; }

        public string Text { get; set; }

        public SelectList(string vValue, string vText)
        {
            Value = vValue;
            Text = vText;
        }
    }

    public class RazorHelperService
    {
        public List<SelectList> SalaahTimesTypeSelectList()
        {
            List<SelectList> List = new List<SelectList>();

            List.Add(new SelectList("1", "Schedule Time Changes"));
            List.Add(new SelectList("2", "Daily Time Records"));

            return List;
        }

        public List<SelectList> JuristicMethodSelectList()
        {
            List<SelectList> List = new List<SelectList>();

            List.Add(new SelectList("1", "University Of Islamic Sciences Karachi"));
            List.Add(new SelectList("2", "Muslim World League"));
            List.Add(new SelectList("3", "Islamic Society Of North America"));
            List.Add(new SelectList("4", "Umm Al Qura University Makkah"));
            List.Add(new SelectList("5", "Egyptian General Authority Of Survey"));

            return List;
        }


    }
}
