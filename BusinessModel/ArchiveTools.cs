using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModel;
using Resources;

namespace BusinessModel
{
    public static class ArchiveTools
    {
        public static string GetContentBase64(this ArchiveFile t)
        {
            var base64 = Convert.ToBase64String(t.Content);
            return string.Format($"data:{t.ContentType};base64,{base64}");
        }
        public static string FullName(this BaseIndividualInformations b)
        {
            return $"{b.FirstName} { b.LastName}";
        }

        public static string GetFullOrganizationPath(this Organization o)
        {
            try
            {


                OrganizationManager organizationManager = new OrganizationManager();
                StringBuilder stb = new StringBuilder();

                stb.Append(o.Name);

                foreach (var item in organizationManager.GetParents(o.Id, new List<Organization>()))
                {
                    stb.Append(" " + item.Name);
                }
                return stb.ToString();
            }
            catch
            {
                return "سازمان ها";
            }

        }

        public static string GetFullDepartmentPath(this Department d)
        {
            try
            {


                DepartmentManager departmentManager = new DepartmentManager();
                StringBuilder stb = new StringBuilder();

                stb.Append(d.Name);

                foreach (var item in departmentManager.GetParents(d.Id, new List<Department>()))
                {
                    stb.Append(" " + item.Name);
                }
                return stb.ToString();
            }
            catch
            {
                return "دپارتمان ها";
            }

        }

        public static string GetAvatar(this BaseIndividualInformations b)
        {
            if (b.Avatar.Content != null && b.Avatar.Content.Length > 0)
            {
                var base64 = Convert.ToBase64String(b.Avatar.Content);
                return string.Format($"data:{b.Avatar.ContentType};base64,{base64}");
            }
            if (b.Gender == Gender.Male)
                return ViewResourecs.DefultProfilePictureMale;
            return ViewResourecs.DefultProfilePictureFemale;
        }
    }
}
