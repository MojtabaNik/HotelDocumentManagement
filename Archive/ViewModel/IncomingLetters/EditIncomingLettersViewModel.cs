using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Archive.ViewModel.Templates;
using EntityModel;

namespace Archive.ViewModel
{
    public class EditIncomingLettersViewModel : BaseViewModel
    {


        // public string OrganizationFilterString { get; set; }

        // public List<Organization> Organizations { get; set; }
        // public List<Organization> FilterOrganizations { get; set; }
        // //add
        // public Guid? Organization { get; set; }


        // public Guid? Person { get; set; }
        // public string PersonFilterString { get; set; }

        // 
        // //add
        // public string PersonListString { get; set; }


        // public string SelectedCasesGuidString { get; set; }
        // public List<ReceivedLetter> ListReciveLetters { get; set; }

        //// public ReceivedLetter ParentReceivedLetter { get; set; }

        // public string ArchiveDateTime { get; set; }
        // public List<string> OlderFilesContent { get; set; }
        // public List<string> OlderFilesName { get; set; }
        // public List<bool> deleted { get; set; }

        public List<Guid> FilesId { get; set; }
        public List<Guid> AppendagesId { get; set; }
        public List<Guid> CasesGuid { get; set; }
        public ReceivedLetter ReceivedLetter { get; set; }
        public string LetterDateTime { get; set; }
        public string RowNumber { get; set; }
        public Guid? ParentletterId { get; set; }
        public List<Post> Posts { get; set; }
        public Guid PostGuid { get; set; }
        public Guid PersonGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
        public List<Person> People { get; set; }
        public List<FileViewModel> LetterFiles { get; set; }
        public List<FileViewModel> LetterAppendages { get; set; }
        public string OrganizationJson { get; set; }
        public List<HttpPostedFileBase> FileUploadLetters { get; set; }
        public List<HttpPostedFileBase> FileUploadAppendages { get; set; }
        public Guid? ParentReceiveletterId { get; set; }
        public List<Case> Cases { get; set; }
    }
}