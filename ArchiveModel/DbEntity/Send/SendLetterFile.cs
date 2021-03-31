using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    [Serializable]
    public class SendLetterFile:EntityBaseModel
    {
        //public string Descrition { get; set; }
        public ArchiveFile File { get; set; }
        public virtual SendLetter SendLetter { get; set; }
    }
}
