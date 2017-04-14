using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approved
{
    class productPreference
    {
        public string likeID { get; set; }
        public string likeUser { get; set; }
        public string likeProductTitle { get; set; }
        public string likePreference { get; set; }

        public int likeMonth { get; set; }
        public int likeDay { get; set; }

        public productPreference(string id, string user, string product, string pref, string date)
        {
            likeID = id;
            likeUser = user;
            likeProductTitle = product;
            likePreference = pref; ;

            convertMonth(date);
            convertDay(date);
        }

        private void convertMonth(string timestamp)
        {
            DateTime date = Convert.ToDateTime(timestamp);
            int month = date.Month;

            likeMonth = month;
        }

        private void convertDay(string timestamp)
        {
            DateTime date = Convert.ToDateTime(timestamp);
            int day = date.Day;

            likeDay = day;
        }
    }
}
