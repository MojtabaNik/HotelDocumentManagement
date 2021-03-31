using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{

    public class SendLetter
    {
        public SendLetter ParentSendLetter { get; set; }
       
        public virtual IList<TempSendLetter> SendLetterHistory { get; set; }

    }
}
