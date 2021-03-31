using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EntityModel;


namespace BusinessModel
{
    public static class Sessions
    {
        public static bool IsAdmin
        {
            get
            {

                if (HttpContext.Current.Session["_isAdmin_"] != null)
                    return (bool)HttpContext.Current.Session["_isAdmin_"];
                    return false;
            }
            set
            {
                HttpContext.Current.Session["_isAdmin_"] = value;
            }
        }

        public static User CurrentUser
        {
            get
            {

                if (HttpContext.Current.Session["_currentUser_"] != null)
                    return (User)HttpContext.Current.Session["_currentUser_"];
                return null;
            }
            set
            {
                HttpContext.Current.Session["_currentUser_"] = value;
            }
        }

        public static string ReturnUrl
        {
            get
            {

                if (HttpContext.Current.Session["_returnUrl_"] != null)
                    return (string)HttpContext.Current.Session["_returnUrl_"];
                return "";
            }
            set
            {
                HttpContext.Current.Session["_returnUrl_"] = value;
            }
        }
    }


}
