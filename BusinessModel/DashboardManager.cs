using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
    public class DashboardManager:DBProvider<SendLetter>
    {
        public DashboardManager():base(new DatabaseContext())
        {

        }
    }
}
