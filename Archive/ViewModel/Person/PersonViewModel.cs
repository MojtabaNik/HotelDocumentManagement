using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModel;

namespace Archive.ViewModel
{
   public class PersonViewModel:BaseViewModel
    {
        public List<Person> People { set; get; }
    }
}
