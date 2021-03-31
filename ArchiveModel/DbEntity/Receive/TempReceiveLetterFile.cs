using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TempReceiveLetterFile:EntityBaseModel
    {
        public TempReceiveLetterFile()
        {
            Active = false;
        }
        //public int TempReceiveLetterFileId { get; set; }
        //public string Descrition { get; set; }
        public ArchiveFile File { get; set; }
        public virtual TempReceivedLetter TempReceiveLetter { get; set; }
    }
}
