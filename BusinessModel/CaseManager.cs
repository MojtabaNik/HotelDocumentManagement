using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
    public class CaseManager:DBProvider<Case>
    {
        public CaseManager() : base(new DatabaseContext())
        {
        }

        public List<Case> GetChildItems(Guid parentId, List<Case> EmptyRole)
        {


            if (GetRowsByParent(parentId).Count > 0) // If this item have childs then i print them
            {
                //Label1.Text += "<ul>";
                foreach (Case row in GetRowsByParent(parentId))
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

        private List<Case> GetRowsByParent(Guid parentId)
        {
            List<Case> DbMenuTable = Read(null);
            List<Case> r = new List<Case>();
            foreach (var item in DbMenuTable)
            {
                if (item.ParentCase?.Id == parentId)
                    r.Add(item);
            }

            return r;
        }


        public Case GetParent(Guid child)
        {
            var parent =
                DBC.Database.SqlQuery<Guid>("SELECT ParentCase_Id from Cases where Id={0}", child)
                    .FirstOrDefault<Guid>();

            return Read(parent);
        }
    }
}
