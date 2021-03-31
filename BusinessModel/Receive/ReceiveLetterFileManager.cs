using DataAccessLayer;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
    public class ReceiveLetterFileManager:DBProvider<ReceiveLetterFile>
    {
        public ReceiveLetterFileManager():base(new DatabaseContext())
        {
            
        }
    }
}
