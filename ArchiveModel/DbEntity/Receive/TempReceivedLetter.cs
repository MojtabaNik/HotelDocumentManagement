using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TempReceivedLetter:EntityBaseModel
    {
        public virtual SendLetter ParentSendLetterOutTemp { get; set; }
        public virtual ReceivedLetter ParentReceiveLetter { get; set; }
        public ParentType ParentType { get; set; }
        public string RowNumber { get; set; }
        public string Description { get; set; }
        public DateTime LetterDateTime { get; set; }
        public DateTime ArchiveDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public virtual IList<TempReceiveAppendage> TempReceiveAppendages { get; set; }
        public LetterAssortment Assortment { get; set; }
        public virtual IList<Case> Cases { get; set; }
        public virtual IList<TempReceiveLetterFile> TempReceiveLetterFiles { get; set; }
        public virtual User User { get; set; }
        public virtual ReceivedLetter ReceivedLetter { get; set; }
        public virtual Sender Sender { get; set; }
        public Byte[] AppendageContent { get; set; }
        public Byte[] LetterFileContent { get; set; }
        public Byte[] LetterFileAppendageContent { get; set; }
    }
}
