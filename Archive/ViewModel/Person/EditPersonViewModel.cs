using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class EditPersonViewModel : BaseViewModel
    {
        public Person Person { get; set; }
        public HttpPostedFileBase fileUpload { get; set; }
        public bool Flag { get; set; }

        public List<Organization> Organizations { get; set; }
        public List<Post> Posts { get; set; }

        public List<Guid> SelectedOrganizations { get; set; }
        public List<Guid> SelectedPosts { get; set; }

        public string BirthDay { get; set; }
    }
}
