using DataAccessLayer;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
   public class SendLetterAppendageManager: DBProvider<SendAppendage>
    {
        public SendLetterAppendageManager():base(new DatabaseContext())
        {

        }
    }
}
