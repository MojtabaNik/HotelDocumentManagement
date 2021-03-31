using DataAccessLayer;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
   public class ReceiveLetterAppendageManager:DBProvider<ReceiveAppendage>
    {
        public ReceiveLetterAppendageManager():base(new DatabaseContext())
        {
            
        }
    }
}
