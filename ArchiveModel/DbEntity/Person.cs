using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace EntityModel
{
    public class Person:BaseIndividualInformations
    {
        public virtual IList<User> Users { get; set; }
        //public virtual IList<Organization> Organizations { get; set; }
        public virtual IList<Post_Organization> PostOrganizations { get; set; }
       
        //public virtual IList<TempReceivedLetter> TempReceiveLetters { get; set; }
      
        //public virtual IList<TempSendLetter> TempSendLetters { get; set; }
    }
}
