using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Archive.ViewModel
{
    public class AddUserViewModel:BaseViewModel
    {
        public User user { get; set; }
        public string repeatPassword { get; set; }
        public HttpPostedFileBase fileUpload { get; set; }
        public string BirthDay { get; set; }
    }
}