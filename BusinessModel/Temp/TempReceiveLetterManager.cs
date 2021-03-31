using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityModel;
using DBProvider;

namespace BusinessModel
{
    public class TempReceiveLetterManager:DBProvider<TempReceivedLetter>
    {
        public TempReceiveLetterManager():base(new DatabaseContext())
        {
            
        }

        public ReceivedLetter ReturnReceivedLetter(Guid receiveLetter)
        {            
            return DBC.Set<ReceivedLetter>().Find(receiveLetter);
        }
       
    }
}
