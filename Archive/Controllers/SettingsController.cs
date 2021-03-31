using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Archive.ViewModel;
using BusinessModel;
using EntityModel;
using Newtonsoft.Json;

namespace Archive.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel { PageId = "Settings" };
            OrganizationManager organizationManager = new OrganizationManager();
            UserManager userManager = new UserManager();
            User currentUser = userManager.Read(new Guid(User.Identity.Name));
            settingsViewModel.Organizations = organizationManager.Read(null);
            settingsViewModel.FilterOrganizations = currentUser.FilterOrganizations.ToList();
            settingsViewModel.OrganizationJson = getWhileLoopData();

            PersonManager personManager = new PersonManager();
            settingsViewModel.People = personManager.NoneOrganization();
            settingsViewModel.FilterPersons = currentUser.FilterPeople.ToList();

            settingsViewModel.OrganizationFilterString = string.Join(",", currentUser.FilterOrganizations.Select(x => x.Id.ToString()));
            settingsViewModel.PersonFilterString = string.Join(",", currentUser.FilterPeople.Select(x => x.Id.ToString()));

            return View(settingsViewModel);
        }
        private StringBuilder htmlStr = new System.Text.StringBuilder();
        public string getWhileLoopData()
        {
            OrganizationManager organizationManager = new OrganizationManager();


            List<Organization> empty = new List<Organization>();
            empty = organizationManager.GetChildItems(organizationManager.Read(null).First().Id, empty);
            htmlStr.Append("[{id:\"");
            htmlStr.Append(organizationManager.Read(null).First().Id);
            htmlStr.Append("\",text: \"");
            htmlStr.Append(organizationManager.Read(null).First().Name);
            htmlStr.Append("\",expanded: true,spriteCssClass: \"rootfolder\"");
            //Check if there is any children or not
            if (GetRowsByParent(organizationManager.Read(null).First().Id).Count > 0)
            {
                htmlStr.Append(",items: [");
                //Les go inside Childs
                GetChildItems(organizationManager.Read(null).First().Id, new List<Organization>());
                //End tag
                htmlStr.Append("]");
            }

            htmlStr.Append("}]");
            return htmlStr.ToString();
        }
        public List<Organization> GetChildItems(Guid parentId, List<Organization> EmptyRole)
        {
            //OrganizationManager organizationManager = new OrganizationManager();
            //var currentOrganization = organizationManager.Read(parentId);
            //htmlStr.Append("{id:\"");
            //htmlStr.Append(currentOrganization.Id);
            //htmlStr.Append("\",text: \"");
            //htmlStr.Append(currentOrganization.Name);
            //htmlStr.Append("\",expanded: true,spriteCssClass: \"folder\"");

            if (GetRowsByParent(parentId).Count > 0) // If this item have childs then i print them
            {
                //htmlStr.Append(",items: [");
                //Label1.Text += "<ul>";
                foreach (Organization row in GetRowsByParent(parentId))
                {
                    EmptyRole.Add(row);

                    htmlStr.Append("{id:\"");
                    htmlStr.Append(row.Id);
                    htmlStr.Append("\",text: \"");
                    htmlStr.Append(row.Name);
                    htmlStr.Append("\",expanded: true,spriteCssClass: \"folder\"");
                    // Label1.Text += "<li>" + row.Role_Description.ToString() + "</li>"; // Print child title
                    //
                    if (GetRowsByParent(row.Id).Count > 0)
                    {
                        htmlStr.Append(",items: [");
                        GetChildItems(row.Id, EmptyRole); // Search for childs that belong to this item
                        htmlStr.Append("]");
                    }
                    htmlStr.Append("},");
                }
                htmlStr.Length--;
                //htmlStr.Append("]");
                //Label1.Text += "</ul>";
            }
            //htmlStr.Append("}");
            return EmptyRole;

        }
        private List<Organization> GetRowsByParent(Guid parentId)
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
        [HttpPost]
        public ActionResult Edit(EditSettingsViewModel editSettingsViewModel)
        {
            List<Guid> GuidList = new List<Guid>();
            UserManager userManager = new UserManager();
            //Get GuidList of Organization from input
            if (!String.IsNullOrEmpty(editSettingsViewModel.OrganizationFilterString))
            {
                List<string> GUIDS = new List<string>();

                if (editSettingsViewModel.OrganizationFilterString.Contains(","))
                {
                    GUIDS = editSettingsViewModel.OrganizationFilterString.Split(',').ToList();
                }
                else
                {
                    GUIDS.Add(editSettingsViewModel.OrganizationFilterString);
                }

                foreach (var item in GUIDS)
                {
                    Guid id = Guid.Parse(item);
                    GuidList.Add(id);
                }
            }
            userManager.AddFilterOrganization(new Guid(User.Identity.Name), GuidList);
            //Reset Guid List
            GuidList = new List<Guid>();
            //Get GuidList of Organization from input
            if (!String.IsNullOrEmpty(editSettingsViewModel.PersonFilterString))
            {
                List<string> GUIDS = new List<string>();

                if (editSettingsViewModel.PersonFilterString.Contains(","))
                {
                    GUIDS = editSettingsViewModel.PersonFilterString.Split(',').ToList();
                }
                else
                {
                    GUIDS.Add(editSettingsViewModel.PersonFilterString);
                }

                foreach (var item in GUIDS)
                {
                    Guid id = Guid.Parse(item);
                    GuidList.Add(id);
                }
            }
            userManager.AddFilterPerson(new Guid(User.Identity.Name), GuidList);
            //Save Things to Database 
            userManager.saveChanges();
            return RedirectToAction("Index");
        }






    }
}