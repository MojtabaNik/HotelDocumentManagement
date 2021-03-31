using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class Organization:EntityBaseModel
    {
        //[Required(ErrorMessageResourceType = typeof(Resources.ViewResourecs), ErrorMessageResourceName = "RequiredField")]
        //[Range(5, 50, ErrorMessageResourceName = "RangeUserName", ErrorMessageResourceType = typeof(Resources.ViewResourecs))]
        public string Name { get; set; }
        public virtual Organization ParentOrganization { get; set; }
        //public virtual IList<Person> People { get; set; }
        //public virtual IList<TempReceivedLetter> TempReceiveLetters { get; set; }
        //public virtual IList<TempSendLetter> TempSendLetters { get; set; }
        public virtual IList<User> Users { get; set; }
    }
}
