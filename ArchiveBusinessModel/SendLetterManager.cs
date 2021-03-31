using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiveModel;
using DataAccessLayer;
using DBProvider;

namespace BusinessModel
{
    public class SendLetterManager:DBProvider<SendLetter>
    {
        public SendLetterManager():base(new DatabaseContext())
        {
            
        }
    }
}
