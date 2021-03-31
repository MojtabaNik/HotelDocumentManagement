using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TempSendLetter
    {
        //
        public SendLetter ParentSendLetter { get; set; }

        //This should be relation
        public virtual SendLetter SendLetter { get; set; }

    }
}
