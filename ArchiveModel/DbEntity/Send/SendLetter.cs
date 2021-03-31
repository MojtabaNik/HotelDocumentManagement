using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    [Serializable]
    public class SendLetter : EntityBaseModel
    {
        public virtual IList<TempReceivedLetter> ParentSendLetterOutTemp { get; set; }
        public virtual IList<ReceivedLetter> ParentSendLetterOut { get; set; }
        public virtual ReceivedLetter ParentReceivedLetterOut { get; set; }

        public virtual SendLetter ParentSendLetter { get; set; }
        public ParentType ParentType { get; set; }
        public string RowNumber { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime LetterDateTime { get; set; }
        public DateTime ArchiveDateTime { get; set; }
        public DateTime LastModifyDateTime { get; set; }
        public virtual IList<SendAppendage> Appendages { get; set; }
        public LetterAssortment Assortment { get; set; }
        public virtual IList<Case> Cases { get; set; }
        public virtual IList<SendLetterFile> sendLetterFiles { get; set; }
        public virtual User User { get; set; }
        public virtual IList<TempSendLetter> TempSendLetters { get; set; }
        public virtual IList<Reciever> Recievers { get; set; }
        public virtual SenderEmployee SenderEmployee { get; set; }
        public byte[] AppendageContent { get; set; }
        public byte[] LetterFileContent { get; set; }
        public byte[] LetterFileAppendageContent { get; set; }

    }
}
