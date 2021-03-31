using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel.OutgoingLetters
{
    public class AddOutgoingViewModel:BaseViewModel
    {

        public Guid? ParentSendletterId { get; set; }
        //public string OrganizationFilterString { get; set; }
        public string OrganizationJson { get; set; }
        //public List<Organization> Organizations { get; set; }
        //public List<Organization> FilterOrganizations { get; set; }

        public List<Post> Posts { get; set; }


        //public string PersonFilterString { get; set; }
        public List<Person> People { get; set; }
        //public List<Person> FilterPersons { get; set; }
        //add
        

        //public string DepartmentString { get; set; }
        public string DepartmentJson { get; set; }
        //add
        public Guid DepartmentGuid { get; set; }


        //public List<Department> Departments { get; set; }

        public SendLetter sendLetter { get; set; }
        public List<HttpPostedFileBase> fileUploadLetters { get; set; }
        public List<HttpPostedFileBase> fileUploadAppendages { get; set; }

        public List<Case> Cases { get; set; }

        public List<Employee> Employees { get; set; }
        //add
        public Guid EmployeeGuid { get; set; }
        public string Content { get; set; }
        public string SelectedCasesGuidString { get; set; }

        public Guid LetterTemplate { get; set; }

        public List<Template> Templates { get; set; }

        //Add PostMethod
        public Guid PersonGuid { get; set; }   //add
        public Guid OrganizationGuid { get; set; }

        public Guid PostGuid { get; set; }
        public string LetterDateTime { get; set; }
        public Guid? ParentletterId { get; set; }

        public Guid DepartmentPostGuid { get; set; }

        public bool autoRowNumber { get; set; }
        public string RowNumber { get; set; }

        public List<Guid> SelectedCasesGuid { get; set; }
    }
}