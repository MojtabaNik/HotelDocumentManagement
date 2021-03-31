using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Archive.ViewModel
{
    public class ShowAllUsersViewModel:BaseViewModel
    {
        public List<User> Users { set; get; }
    }
}