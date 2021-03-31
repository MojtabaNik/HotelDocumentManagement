using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    [Serializable]
    public class ArchiveFile
    {
        public string Name { get; set; }
        public Byte[] Content { get; set; }
        /// <summary>
        /// Like Content/Jpg or Content/pdf
        /// </summary>
        public string ContentType { get; set; }
    }
}
