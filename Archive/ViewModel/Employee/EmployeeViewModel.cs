using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class EmployeeViewModel : BaseViewModel
    {
        public List<Employee> Employees { get; set; }
    }
}