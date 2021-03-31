using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class Sender : EntityBaseModel
    {
        public virtual Person Person { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Post Post { get; set; }
        public virtual IList<ReceivedLetter> ReceiveLetters { get; set; }
        public virtual IList<TempReceivedLetter> TempReceiveLetters { get; set; }
    }
}
