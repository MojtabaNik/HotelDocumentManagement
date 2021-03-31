using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class Department:EntityBaseModel
    {
        //[Required(ErrorMessageResourceType = typeof(Resources.ViewResourecs), ErrorMessageResourceName = "RequiredField")]
        //[Range(5, 50, ErrorMessageResourceName = "RangeUserName", ErrorMessageResourceType = typeof(Resources.ViewResourecs))]
        public string Name { get; set; }
        public virtual Department ParentDepartement { get; set; }
        public virtual IList<SendLetter> Sendletters { get; set; }
        public virtual IList<TempSendLetter> TempSendLetters { get; set; }
        public virtual IList<Employee> Employees { get; set; }
    }
}
