using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
    public class TemplateManager : DBProvider<Template>
    {
        public TemplateManager() : base(new DatabaseContext())
        {
        }
    }
}
