using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
   public class Post_Department: EntityBaseModel
    {
        public virtual Post Post { get; set; }

        public virtual Department Department { get; set; }

        public virtual IList<Employee> Employees { get; set; }
    }
}
