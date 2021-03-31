using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class Post_Organization:EntityBaseModel
    {
        public virtual Post Post { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual IList<Person> People { get; set; }
    }
}
