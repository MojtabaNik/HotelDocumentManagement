using EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class Case:EntityBaseModel
    {
        //[Required(ErrorMessageResourceType = typeof(Resources.ViewResourecs), ErrorMessageResourceName = "RequiredField")]
        //[Range(5, 50, ErrorMessageResourceName = "RangeUserName", ErrorMessageResourceType = typeof(Resources.ViewResourecs))]
        public string Name { get; set; }
        public virtual Case ParentCase { get; set; }
        public virtual IList<ReceivedLetter> ReceiveLetters { get; set; }
        public virtual IList<SendLetter> SendLetters { get; set; }
        public virtual IList<TempReceivedLetter> TempReceiveLetters { get; set; }
        public virtual IList<TempSendLetter> TempSendLetters { get; set; }
    }
}
