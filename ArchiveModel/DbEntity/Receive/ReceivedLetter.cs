using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModel
{
    public class ReceivedLetter:EntityBaseModel
    {
        public virtual IList<TempSendLetter> ParentReceivedLetterOutTemp { get; set; }
        public virtual IList<SendLetter> ParentReceivedLetterOut { get; set; }
        public virtual SendLetter ParentSendLetterOut { get; set; }
        public virtual ReceivedLetter ParentReceiveLetter { get; set; }
        public ParentType ParentType { get; set; }
        /// <summary>
        /// Letter Number 
        /// </summary>
        public string RowNumber { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime LetterDateTime { get; set; }
        public DateTime ArchiveDateTime { get; set; }
        public DateTime LastModifyDateTime { get; set; }
        public virtual IList<ReceiveAppendage> Appendages { get; set; }
        public LetterAssortment Assortment { get; set; }
        public virtual IList<Case> Cases { get; set; }
        public virtual IList<ReceiveLetterFile> ReceiveLetterFiles { get; set; }
        public virtual User User { get; set; }
        public virtual IList<TempReceivedLetter> TempReceiveLetters { get; set; }
        public virtual Sender Sender { get; set; }
        public Byte[] AppendageContent { get; set; }
        public Byte[] LetterFileContent { get; set; }
        public Byte[] LetterFileAppendageContent { get; set; }
    }
}
