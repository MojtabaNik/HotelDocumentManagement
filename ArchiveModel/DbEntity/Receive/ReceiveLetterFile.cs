using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class ReceiveLetterFile:EntityBaseModel
    {
        //public string Descrition { get; set; }
        public ArchiveFile File { get; set; }
        public virtual ReceivedLetter ReceiveLetter { get; set; }

    }
}
