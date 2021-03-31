using System.Collections.Generic;

namespace EntityModel
{
    public class Reciever : EntityBaseModel
    {
        public virtual Person Person { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Post Post { get; set; }
        public virtual IList<TempSendLetter> TempSendLetters { get; set; }
        public virtual IList<SendLetter> SendLetters { get; set; }

    }
}
