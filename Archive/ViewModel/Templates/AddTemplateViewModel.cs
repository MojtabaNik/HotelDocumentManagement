using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class AddTemplateViewModel : BaseViewModel
    {
        public HttpPostedFileBase fileUpload { get; set; }

        public Template template { get; set; }
    }
}
