using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityModel
{
    public class PrintableSendLetter
    {
        public string DateTime { get; set; }
        public string Number { get; set; }
        public string Appendage { get; set; }
        public string Body { get; set; }
        public string SenderName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string DepartMentName { get; set; }
    }
}