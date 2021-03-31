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
    public class ActivityLogManager:DBProvider<ActivityLog>
    {
        public ActivityLogManager():base(new DatabaseContext())
        {

        }
        /// <summary>
        /// Send letter loging for Create letter action
        /// </summary>
        public void CreateLog()
        {
            //Add(new ActivityLog { RowId = Entity.LetterId, TableName = "ReceiveLetter", Action = action.Create, Description = $"Create new outgoing Letter with row number :{sendLetter.RowNumber}", User = sendLetter.User });
            //saveChanges();
        }
    }
}
