using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Archive.ViewModel
{
    public class OrganizationViewModel:BaseViewModel
    {
        public List<Organization> Organization { get; set; }
        public string Name { get; set; }
        public Guid ParentOrganizationGUID { get; set; }

        public Organization SelectedOrganization { get; set; }
        public Organization SelectedOrganizationParent { get; set; }

        public string OrganizationJson { get; set; }

        public string OrganizationString { get; set; }
    }
}