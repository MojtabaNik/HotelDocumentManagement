using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityModel
{
   
    public class User : BaseIndividualInformations
    {
        //[Required(ErrorMessageResourceType = typeof(Resources.ViewResourecs),ErrorMessageResourceName = "RequiredField")]
        //[Range(5,50)]//ErrorMessageResourceName = "RangeUserName", ErrorMessageResourceType = typeof(Resources.ViewResourecs))]
        //[Range(1, 1000,ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public User()
        {
           
        }

        public string UserName { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.ViewResourecs), ErrorMessageResourceName = "RequiredField")]
        //[Range(3, 50, ErrorMessageResourceName = "RangeUserName", ErrorMessageResourceType = typeof(Resources.ViewResourecs))]
        public string password { get; set; }

        public bool IsAdmin { get; set; }
        /// <summary>
        /// Like Admin-Clerk-Developer
        /// </summary>
        public Role Role { get; set; }
        public virtual IList<ActivityLog> Activities { get; set; }
        public virtual IList<ReceivedLetter> ReceiveLetters { get; set; }
        public virtual IList<SendLetter> SendLetters { get; set; }
        public virtual IList<TempReceivedLetter> TempReceiveLetters { get; set; }
        public virtual IList<TempSendLetter> TempSendLetters { get; set; }
        public virtual IList<Organization> FilterOrganizations { get; set; }
        public virtual IList<Person> FilterPeople { get; set; }
    }
}
