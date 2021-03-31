using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TempSendLetterFile:EntityBaseModel
    {
        public TempSendLetterFile()
        {
            Active = false;
        }
        //public string Descrition { get; set; }
        public ArchiveFile File { get; set; }
        public virtual TempSendLetter TempSendLetter { get; set; }
    }
}
