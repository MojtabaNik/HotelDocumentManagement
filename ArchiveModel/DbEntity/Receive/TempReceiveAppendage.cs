using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TempReceiveAppendage:EntityBaseModel
    {
        public TempReceiveAppendage()
        {
            Active = false;
        }
        //public string Descrition { get; set; }
        public ArchiveFile File { get; set; }
        public virtual TempReceivedLetter TempReceiveLetter { get; set; }
    }
}
