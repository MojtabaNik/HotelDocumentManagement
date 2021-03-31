using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class PostViewModel:BaseViewModel
    {
        public List<Post> posts { get; set; }
        public Post post { get; set; }
        //Edit Section
        public bool isEdit { get; set; }
        public Guid id { get; set; }
    }
}