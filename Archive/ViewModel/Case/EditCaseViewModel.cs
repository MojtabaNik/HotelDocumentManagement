using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Archive.ViewModel
{
    public class EditCaseViewModel
    {
        public Guid CaseID { get; set; }
        public Guid ParentCaseGUID { get; set; }
        public string Name { get; set; }
    }
}