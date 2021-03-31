using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Archive.ViewModel;
using BusinessModel;
using DBProvider;
using EntityModel;

namespace Archive.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            DepartementViewModel departementViewModel = new DepartementViewModel
            {
                Departements = departmentManager.Read(null),
                PageId = "Department"
            };
            departementViewModel.DepartmentJson = getWhileLoopJsonDepartment(false, new Guid());
            departementViewModel.DepartmentString = string.Join(",", departmentManager.Read(null).Select(x => x.Id.ToString()));
            return View(departementViewModel);
        }

        //GetJson for TreeView
        private System.Text.StringBuilder htmlStr2 = new System.Text.StringBuilder();
        private string getWhileLoopJsonDepartment(bool isEdit, Guid parent)
        {
            DepartmentManager departmentManager = new DepartmentManager();


            List<Department> empty = new List<Department>();
            empty = departmentManager.GetChildItems(departmentManager.Read(null).First().Id, empty);
            htmlStr2.Append("[{id:\"");
            htmlStr2.Append(departmentManager.Read(null).First().Id);
            htmlStr2.Append("\",text: \"");
            htmlStr2.Append(departmentManager.Read(null).First().Name);
            htmlStr2.Append("\",expanded: true,spriteCssClass: \"rootfolder\"");
            //Check if there is any children or not
            if (GetRowsByParentDep(departmentManager.Read(null).First().Id).Count > 0)
            {
                htmlStr2.Append(",items: [");
                //Les go inside Childs
                if (isEdit)
                {
                    if (departmentManager.Read(null).First().Id != parent)
                        GetChildItems(departmentManager.Read(null).First().Id, new List<Department>(), isEdit, parent);
                }
                else
                {
                    GetChildItems(departmentManager.Read(null).First().Id, new List<Department>(), isEdit, parent);
                }
                //End tag
                htmlStr2.Append("]");
            }

            htmlStr2.Append("}]");
            return htmlStr2.ToString();
        }

        private List<Department> GetRowsByParentDep(Guid parentId)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            List<Department> DbMenuTable = departmentManager.Read(null);
            List<Department> r = new List<Department>();
            foreach (var item in DbMenuTable)
            {
                if (item.ParentDepartement?.Id == parentId)
                    r.Add(item);
            }

            return r;
        }

        private List<Department> GetChildItems(Guid parentId, List<Department> EmptyRole, bool isEdit, Guid parent)
        {



            if (GetRowsByParentDep(parentId).Count > 0) // If this item have childs then i print them
            {
                bool isSomethingAdded = false;
                foreach (Department row in GetRowsByParentDep(parentId))
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
                if (isSomethingAdded)
                htmlStr2.Length--;
            }
            return EmptyRole;
        }

        //End Department Json

        public string getWhileLoopData()
        {
            DepartmentManager departmentManager = new BusinessModel.DepartmentManager();

            var htmlStr = new System.Text.StringBuilder();
            List<Department> empty = new List<Department>();
            empty = departmentManager.GetChildItems(departmentManager.Read(null).First().Id, empty);
            htmlStr.Append("<tr><td>");
            htmlStr.Append(departmentManager.Read(null).First().Id);
            htmlStr.Append("</td><td>");
            htmlStr.Append("");
            htmlStr.Append("</td><td>");
            htmlStr.Append(departmentManager.Read(null).First().Name);
            htmlStr.Append("</td></tr>");

            foreach (var item in empty)
            {
                string parent = "";
                if (item.ParentDepartement != null)
                {
                    parent = item.ParentDepartement.Id.ToString();
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
        public ActionResult Add(DepartementViewModel departementViewModel)
        {
            DepartmentManager departementManager = new DepartmentManager();
            Department ParentDepartment = new Department();
            Department newDepartement = new Department();
            ParentDepartment = departementManager.Read(x => x.Id == departementViewModel.ParentDepartmentGUID).First();
            newDepartement.Name = departementViewModel.Name;
            newDepartement.ParentDepartement = ParentDepartment;
            departementManager.Add(newDepartement);
            departementManager.saveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id)
        {
            DepartmentManager departementManager = new DepartmentManager();
            DepartementViewModel departementViewModel = new DepartementViewModel() { Departements = departementManager.Read(null) };
            departementViewModel.PageId = "Department";
            departementViewModel.SelectedDepartment = departementManager.Read(id);
            departementViewModel.SelectedDepartmentParent = departementManager.GetParent(id);
            departementViewModel.DepartmentJson = getWhileLoopJsonDepartment(true, departementViewModel.SelectedDepartment.Id);
            departementViewModel.DepartmentString = departementViewModel.SelectedDepartmentParent.Id.ToString();
            return View(departementViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EditDepartmentViewModel editDepartmentViewModel)
        {
            DepartmentManager departementManager = new DepartmentManager();
            var oldDepartment = departementManager.Read(editDepartmentViewModel.DepartmentID);
            oldDepartment.Name = editDepartmentViewModel.Name;
            oldDepartment.ParentDepartement =
                departementManager.Read(editDepartmentViewModel.ParentDepartmentGUID);
            departementManager.Update(oldDepartment);

            departementManager.saveChanges();
            return RedirectToAction("Index");
        }
    }
}