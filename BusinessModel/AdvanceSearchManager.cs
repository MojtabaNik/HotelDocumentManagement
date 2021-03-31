using DataAccessLayer;
using DBProvider;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    public class AdvanceSearchManager:DBProvider<EntityBaseModel>
    {
        public AdvanceSearchManager():base(new DatabaseContext())
        {

        }
    }
}
