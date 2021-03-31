using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Archive.ViewModel
{
    public class DashboardViewModel:BaseViewModel
    {
        public int CountOfLetters { get { return CountOfReceiveLetter + CountOfSendLetter; }}
        public int CountOfReceiveLetter { get; set; }
        public int CountOfSendLetter { get; set; }
        public int CountOfUser { get; set; }
        public IList<ActivityLog> activityLog { get; set; }
    }
}