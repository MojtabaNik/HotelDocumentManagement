using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class EditOutgoingLettersViewModel:BaseViewModel
    {
        public SendLetter sendLetter { get; set; }
        public string OrganizationJson { get; set; }
        //add

        public List<Person> People { get; set; }

        public string DepartmentJson { get; set; }


        public List<Employee> Employees { get; set; }
        public Guid EmployeeGuid { get; set; }

        public List<Case> Cases { get; set; }
        public List<HttpPostedFileBase> fileUploadLetters { get; set; }
        public List<HttpPostedFileBase> fileUploadAppendages { get; set; }

        public string LetterDateTime { get; set; }
        public List<FileViewModel> LetterFiles { get; set; }
        public List<FileViewModel> LetterAppendages { get; set; }

        public List<Guid> FilesID { get; set; }
        public List<Guid> AppendagesID { get; set; }
        public List<Template> Templates { get; set; }

        public List<Post> Posts { get; set; }
        public List<Guid> SelectedCasesGuid { get; set; }
        public Guid? ParentletterId { get; set; }
        public Guid DepartmentPostGuid { get; set; }

        public bool autoRowNumber { get; set; }
        public string RowNumber { get; set; }
        public Guid LetterTemplate { get; set; }
        public Guid PersonGuid { get; set; }   //add
        public Guid OrganizationGuid { get; set; }

        public Guid PostGuid { get; set; }
        public Guid DepartmentGuid { get; set; }
    }
}