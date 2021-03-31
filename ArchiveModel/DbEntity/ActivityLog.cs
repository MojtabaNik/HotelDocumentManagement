using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class ActivityLog:EntityBaseModel
    {
        public long RowId { get; set; }
        public string TableName { get; set; }
        public action Action { get; set; }
        public string Description { get; set; }
        public string GetClass()
        {
            if (Action == action.Create)
            {
                return "warning";
            }
            else if(Action == action.Delete)
            {
                return "warning";
            }
            else if (Action == action.Modify)
            {
                return "info";
            }
            return "";
        }
        public virtual User User { get; set; }

    }
}
