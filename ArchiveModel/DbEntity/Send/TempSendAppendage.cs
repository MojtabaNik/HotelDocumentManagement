using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TempSendAppendage:EntityBaseModel
    {
        public TempSendAppendage()
        {
            Active = false;
        }
        //public int TempSendAppendageId { get; set; }
        //public string Descrition { get; set; }
        public ArchiveFile File { get; set; }
        public virtual TempSendLetter TempSendLetter { get; set; }
    }
}
