using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class SettingsViewModel:BaseViewModel
    {
        public List<Organization> Organizations { get; set; }
        public List<Organization> FilterOrganizations { get; set; }
        public List<Person> People { get; set; }
        public List<Person> FilterPersons { get; set; }

        public string OrganizationFilterString { get; set; }

        public string PersonFilterString { get; set; }

        public string OrganizationJson { get; set; }  
    }
}