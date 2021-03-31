
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public  class BaseIndividualInformations : EntityBaseModel
    {
        //[Required(ErrorMessageResourceType = typeof(Resources.ViewResourecs), ErrorMessageResourceName = "RequiredField")]
        //[Required(ErrorMessage = "Enter First Name")]
        //[Range(5, 50, ErrorMessageResourceName = "RangeUserName", ErrorMessageResourceType = typeof(Resources.ViewResourecs))]
        public string FirstName { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.ViewResourecs), ErrorMessageResourceName = "RequiredField")]
        //[Range(5, 50, ErrorMessageResourceName = "RangeUserName", ErrorMessageResourceType = typeof(Resources.ViewResourecs))]
        public string LastName { get; set; }
        /// <summary>
        /// 0:Female   1:Male
        /// </summary>
        public Gender Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }
        public string Summary { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public ArchiveFile Avatar { get; set; }

        //public string FullName()
        //{
        //    return $"{FirstName} { LastName}";
        //}
        //public string GetAvatar()
        //{
        //    if (Avatar.Content != null && Avatar.Content.Length > 0)
        //    {
        //        var base64 = Convert.ToBase64String(Avatar.Content);
        //        return string.Format($"data:{Avatar.ContentType};base64,{base64}");
        //    }
        //    if (Gender == Gender.Male)
        //        return ViewResourecs.DefultProfilePictureMale;
        //    return ViewResourecs.DefultProfilePictureFemale;
        //}
    }
}
