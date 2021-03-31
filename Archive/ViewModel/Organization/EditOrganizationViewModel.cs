using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Archive.ViewModel
{
    public class EditOrganizationViewModel
    {
        public Guid OrganizationID { get; set; }
        public Guid ParentOrganizationGUID { get; set; }
        public string Name { get; set; }

    }
}