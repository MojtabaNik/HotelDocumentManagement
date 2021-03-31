using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
    public class TempSendLetterManager:DBProvider<TempSendLetter>
    {
        public TempSendLetterManager():base(new DatabaseContext())
        {

        }

        public SendLetter ReturnReceivedLetter(Guid sendLetter)
        {
            return DBC.Set<SendLetter>().Find(sendLetter);
        }
    }
}
