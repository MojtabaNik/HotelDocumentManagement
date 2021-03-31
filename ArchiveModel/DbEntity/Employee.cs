using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;

namespace EntityModel
{
    public class Employee:BaseIndividualInformations
    {
        public virtual IList<Post_Department> PostDepartments { get; set; }
        //public virtual IList<Department> Departments { get; set; }
        //public virtual IList<SendLetter> SendLetters { get; set; }
        //public virtual IList<TempSendLetter> TempSendLetters { get; set; }
    }
}
