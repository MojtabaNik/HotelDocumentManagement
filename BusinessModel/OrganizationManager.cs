using DataAccessLayer;
using DBProvider;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    public class OrganizationManager : DBProvider<Organization>
    {
        public OrganizationManager() : base(new DatabaseContext())
        {
        }

        public List<Organization> GetChildItems(Guid parentId, List<Organization> EmptyRole)
        {


            if (GetRowsByParent(parentId).Count > 0) // If this item have childs then i print them
            {
                //Label1.Text += "<ul>";
                foreach (Organization row in GetRowsByParent(parentId))
                {
                    EmptyRole.Add(row);
                    // Label1.Text += "<li>" + row.Role_Description.ToString() + "</li>"; // Print child title
                    //
                    GetChildItems(row.Id, EmptyRole); // Search for childs that belong to this item
                }
                //Label1.Text += "</ul>";
            }
            return EmptyRole;

        }

        private List<Organization> GetRowsByParent(Guid parentId)
        {
            List<Organization> DbMenuTable = Read(null);
            List<Organization> r = new List<Organization>();
            foreach (var item in DbMenuTable)
            {
                if (item.ParentOrganization?.Id == parentId)
                    r.Add(item);
            }

            return r;
        }

        public Organization GetParent(Guid child)
        {
            var parent =
                DBC.Database.SqlQuery<Guid>("SELECT ParentOrganization_Id from Organizations where Id={0}", child)
                    .FirstOrDefault<Guid>();

            return Read(parent);
        }


        public List<Organization> GetParents(Guid child, List<Organization> organizations)
        {
            var parent = GetParent(child);
            if (parent?.ParentOrganization != null)
            {
                organizations.Add(parent);
                GetParents(parent.Id, organizations);
            }
            return organizations;
        }


    }
}
