using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Archive.ViewModel
{
    public class EditDepartmentViewModel
    {
        public Guid DepartmentID { get; set; }
        public Guid ParentDepartmentGUID { get; set; }
        public string Name { get; set; }
    }
}