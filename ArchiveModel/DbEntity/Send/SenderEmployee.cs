using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class SenderEmployee : EntityBaseModel
    {
        public virtual Employee Employee { get; set; }
        public virtual Department Department { get; set; }
        public virtual Post Post { get; set; }
        public virtual IList<TempSendLetter> TempSendLetters { get; set; }
        public virtual IList<SendLetter> SendLetters { get; set; }

    }
}
