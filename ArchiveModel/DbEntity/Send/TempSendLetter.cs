using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class TempSendLetter:EntityBaseModel
    {
        public virtual ReceivedLetter ParentReceivedLetterOutTemp { get; set; }
        public SendLetter ParentSendLetter { get; set; }
        public ParentType ParentType { get; set; }
        public string RowNumber { get; set; }
        public string PastRowNumber { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime LetterDateTime { get; set; }
        /// <summary>
        /// This Prop is equivalent of Archive DateTime of SendLetter class
        /// </summary>
        public DateTime ArchiveDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public virtual IList<TempSendAppendage> Appendages { get; set; }
        public LetterAssortment Assortment { get; set; }
        public virtual IList<Case> Cases { get; set; }
        public virtual IList<TempSendLetterFile> TempSendLetterFiles { get; set; }
        public virtual SendLetter SendLetter { get; set; }
        public virtual User User { get; set; }
        public virtual IList<Reciever> Recievers { get; set; }
        public virtual SenderEmployee SenderEmployee { get; set; }
        public Byte[] AppendageContent { get; set; }
        public Byte[] LetterFileContent { get; set; }
        public Byte[] LetterFileAppendageContent { get; set; }
    }
}
