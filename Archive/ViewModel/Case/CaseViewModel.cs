using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class CaseViewModel:BaseViewModel
    {
        public List<Case> Case { get; set; }
        public  string Name { get; set; }
        public Guid ParentCaseGUID { get; set; }
        public Case SelectedCase { get; set; }
        public Case SelectedCaseParent { get; set; }
    }
}