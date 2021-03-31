using Archive.ViewModel;
using BusinessModel;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Archive.Controllers
{
    public class OrganizationController : Controller
    {
        // GET: Organization
        public ActionResult Index()
        {
            OrganizationManager organizationManager = new OrganizationManager();
            OrganizationViewModel organizationViewModel = new OrganizationViewModel { Organization = organizationManager.Read(null) };
            organizationViewModel.OrganizationJson = getWhileLoopJsonDepartment(false, new Guid());
            organizationViewModel.OrganizationString = string.Join(",", organizationManager.Read(null).Select(x => x.Id.ToString()));
            organizationViewModel.PageId = "Organization";
            return View(organizationViewModel);
        }

        //GetJson for TreeView
        private System.Text.StringBuilder htmlStr2 = new System.Text.StringBuilder();
        private string getWhileLoopJsonDepartment(bool isEdit, Guid parent)
        {
            OrganizationManager organizationManager = new OrganizationManager();


            List<Organization> empty = new List<Organization>();
            empty = organizationManager.GetChildItems(organizationManager.Read(null).First().Id, empty);
            htmlStr2.Append("[{id:\"");
            htmlStr2.Append(organizationManager.Read(null).First().Id);
            htmlStr2.Append("\",text: \"");
            htmlStr2.Append(organizationManager.Read(null).First().Name);
            htmlStr2.Append("\",expanded: true,spriteCssClass: \"rootfolder\"");
            //Check if there is any children or not
            if (GetRowsByParentDep(organizationManager.Read(null).First().Id).Count > 0)
            {
                htmlStr2.Append(",items: [");
                //Les go inside Childs
                if (isEdit)
                {
                    if(organizationManager.Read(null).First().Id!=parent)
                    GetChildItems(organizationManager.Read(null).First().Id, new List<Organization>(), isEdit, parent);
                }
                else
                {
                    GetChildItems(organizationManager.Read(null).First().Id, new List<Organization>(), isEdit, parent);
                }
                //End tag
                htmlStr2.Append("]");
            }

            htmlStr2.Append("}]");
            return htmlStr2.ToString();
        }

        private List<Organization> GetRowsByParentDep(Guid parentId)
        {
            OrganizationManager organizationManager = new OrganizationManager();
            List<Organization> DbMenuTable = organizationManager.Read(null);
            List<Organization> r = new List<Organization>();
            foreach (var item in DbMenuTable)
            {
                if (item.ParentOrganization?.Id == parentId)
                    r.Add(item);
            }

            return r;
        }

        private List<Organization> GetChildItems(Guid parentId, List<Organization> EmptyRole, bool isEdit, Guid parent)
        {


            if (GetRowsByParentDep(parentId).Count > 0) // If this item have childs then i print them
            {
                bool isSomethingAdded = false;
                foreach (Organization row in GetRowsByParentDep(parentId))
                {
                    if (isEdit)
                    {
                        if (row.Id != parent)
                        {
                            isSomethingAdded = true;
                            EmptyRole.Add(row);

                            htmlStr2.Append("{id:\"");
                            htmlStr2.Append(row.Id);
                            htmlStr2.Append("\",text: \"");
                            htmlStr2.Append(row.Name);
                            htmlStr2.Append("\",expanded: true,spriteCssClass: \"folder\"");
                            if (GetRowsByParentDep(row.Id).Count > 0)
                            {
                                htmlStr2.Append(",items: [");
                                GetChildItems(row.Id, EmptyRole, isEdit, parent);
                                // Search for childs that belong to this item
                                htmlStr2.Append("]");
                            }
                            htmlStr2.Append("},");
                        }
                    }
                    else
                    {
                        isSomethingAdded = true;
                        EmptyRole.Add(row);

                        htmlStr2.Append("{id:\"");
                        htmlStr2.Append(row.Id);
                        htmlStr2.Append("\",text: \"");
                        htmlStr2.Append(row.Name);
                        htmlStr2.Append("\",expanded: true,spriteCssClass: \"folder\"");
                        if (GetRowsByParentDep(row.Id).Count > 0)
                        {
                            htmlStr2.Append(",items: [");
                            GetChildItems(row.Id, EmptyRole, isEdit, parent);
                            // Search for childs that belong to this item
                            htmlStr2.Append("]");
                        }
                        htmlStr2.Append("},");
                    }
                }
                if(isSomethingAdded)
                htmlStr2.Length--;
            }
            return EmptyRole;
        }

        //End Department Json


        public string getWhileLoopData()
        {
            OrganizationManager organizationManager = new OrganizationManager();

            var htmlStr = new System.Text.StringBuilder();
            List<Organization> empty = new List<Organization>();
            empty = organizationManager.GetChildItems(organizationManager.Read(null).First().Id, empty);
            htmlStr.Append("<tr><td>");
            htmlStr.Append(organizationManager.Read(null).First().Id);
            htmlStr.Append("</td><td>");
            htmlStr.Append("");
            htmlStr.Append("</td><td>");
            htmlStr.Append(organizationManager.Read(null).First().Name);
            htmlStr.Append("</td></tr>");
            foreach (var item in empty)
            {
                string parent = "";
                if (item.ParentOrganization != null)
                {
                    parent = item.ParentOrganization.Id.ToString();
                }
                Guid id = item.Id;
                string name = item.Name;
                htmlStr.Append("<tr><td>");
                htmlStr.Append(id);
                htmlStr.Append("</td><td>");
                htmlStr.Append(parent);
                htmlStr.Append("</td><td>");
                htmlStr.Append(name);
                htmlStr.Append("</td></tr>");

            }
            return htmlStr.ToString();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Add(OrganizationViewModel organizationViewModel)
        {
            OrganizationManager organizationManager = new OrganizationManager();
            Organization ParentOrganization = new Organization();
            Organization newOrganization = new Organization();
            ParentOrganization = organizationManager.Read(x => x.Id == organizationViewModel.ParentOrganizationGUID).First();
            newOrganization.Name = organizationViewModel.Name;
            newOrganization.ParentOrganization = ParentOrganization;
            organizationManager.Add(newOrganization);
            organizationManager.saveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id)
        {
            OrganizationManager organizationManager = new OrganizationManager();
            OrganizationViewModel organizationViewModel = new OrganizationViewModel { Organization = organizationManager.Read(null) };
            organizationViewModel.PageId = "Organization";
            organizationViewModel.SelectedOrganization = organizationManager.Read(id);
            organizationViewModel.SelectedOrganizationParent = organizationManager.GetParent(id);
            organizationViewModel.OrganizationJson = getWhileLoopJsonDepartment(true, organizationViewModel.SelectedOrganization.Id);
            organizationViewModel.OrganizationString = organizationViewModel.SelectedOrganizationParent.Id.ToString();
            return View(organizationViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EditOrganizationViewModel editOrganizationViewModel)
        {
            OrganizationManager organizationManager = new OrganizationManager();
            var oldOrganization = organizationManager.Read(editOrganizationViewModel.OrganizationID);
            oldOrganization.Name = editOrganizationViewModel.Name;
            oldOrganization.ParentOrganization =
                organizationManager.Read(editOrganizationViewModel.ParentOrganizationGUID);
            organizationManager.Update(oldOrganization);

            organizationManager.saveChanges();
            return RedirectToAction("Index");
        }
    }
}