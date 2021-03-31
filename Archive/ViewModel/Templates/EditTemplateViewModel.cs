using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class EditTemplateViewModel:BaseViewModel
    {
        public HttpPostedFileBase fileUpload { get; set; }

        public Template template { get; set; }

        public string PreViewImage { get; set; }
    }
}