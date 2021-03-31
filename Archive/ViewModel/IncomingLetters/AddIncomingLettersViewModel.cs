using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel.IncomingLeters
{
    public class AddIncomingLettersViewModel : BaseViewModel
    {
        //Add Get Method
        public string OrganizationJson { get; set; }
        public List<Post> Posts { get; set; }
        public List<Case> Cases { get; set; }
        public List<Person> People { get; set; }
        //Add Post Method
        public ReceivedLetter receivedLetter { get; set; }
        public string LetterDateTime { get; set; }
        public List<HttpPostedFileBase> FileUploadLetters { get; set; }
        public List<HttpPostedFileBase> FileUploadAppendages { get; set; }
        public string RowNumber { get; set; }
        public Guid? ParentletterId { get; set; }
        public Guid PersonGuid { get; set; }
        public Guid OrganizationGuid { get; set; }
        public Guid PostGuid { get; set; }
        public List<Guid> SelectedCasesGuid { get; set; }
    }
}