using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class SystemLog
    {
        public int Id { get; set; }
        public string Exception { get; set; }
        public DateTime ThrowExceptionDataTime { get; set; }
    }
}
