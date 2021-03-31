using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModel;

namespace Archive.ViewModel
{
    public class DepartementViewModel:BaseViewModel
    {
        public List<Department> Departements { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public Guid ParentDepartmentGUID { get; set; }

        public Department SelectedDepartment { get; set; }
        public Department SelectedDepartmentParent { get; set; }

        public string DepartmentJson { get; set; }

        public string DepartmentString { get; set; }
    }
}
