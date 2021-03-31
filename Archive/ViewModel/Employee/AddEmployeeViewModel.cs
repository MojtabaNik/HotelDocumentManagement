using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
   public class AddEmployeeViewModel:BaseViewModel
    {
        public Employee Employee { get; set; }
        public HttpPostedFileBase FileUpload { get; set; }
        public List<Department> Departments { get; set; }
        public List<Guid> SelectedDepartments { get; set; }
        public List<Guid> SelectedPosts { get; set; }
        public string BirthDay { get; set; }
       public List<Post> Posts { get; set; }
    }
}
