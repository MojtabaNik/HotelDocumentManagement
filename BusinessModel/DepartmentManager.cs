using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityModel;
using DBProvider;

namespace BusinessModel
{
    public class DepartmentManager:DBProvider<Department>
    {
        public DepartmentManager() : base(new DatabaseContext())
        {
        }
        public List<Department> GetChildItems(Guid parentId, List<Department> EmptyRole)
        {
            if (GetRowsByParent(parentId).Count > 0) // If this item have childs then i print them
            {
                //Label1.Text += "<ul>";
                foreach (Department row in GetRowsByParent(parentId))
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

        private List<Department> GetRowsByParent(Guid parentId)
        {
            List<Department> DbMenuTable = Read(null);
            List<Department> r = new List<Department>();
            foreach (var item in DbMenuTable)
            {
                if (item.ParentDepartement?.Id == parentId)
                    r.Add(item);
            }
            return r;
        }
        public Department GetParent(Guid child)
        {
            var parent =
                DBC.Database.SqlQuery<Guid>("SELECT ParentDepartement_Id from Departments where Id={0}", child)
                    .FirstOrDefault<Guid>();

            return Read(parent);
        }
        public List<Department> GetParents(Guid child, List<Department> departments)
        {
            var parent = GetParent(child);
            if (parent?.ParentDepartement != null)
            {
                departments.Add(parent);
                GetParents(parent.Id, departments);
            }
            return departments;
        }

    }
}
