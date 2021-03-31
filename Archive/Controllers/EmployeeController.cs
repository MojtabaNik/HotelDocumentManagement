using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Archive.ViewModel;
using BusinessModel;
using DBProvider;
using EntityModel;

namespace Archive.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            EmployeeManager employeeManager = new EmployeeManager();
            EmployeeViewModel employeeViewModel = new EmployeeViewModel
            {
                Employees = employeeManager.Read(null),
                PageId = "ShowEmployee"
            };
            employeeManager.Dispose();
            return View(employeeViewModel);
        }
        [HttpGet]
        public ActionResult Add()
        {
            AddEmployeeViewModel addEmployeeViewModel = new AddEmployeeViewModel {PageId = "AddEmployee"};
            DepartmentManager departmentManager = new DepartmentManager();
            PostManager postManager = new PostManager();
            addEmployeeViewModel.Departments = departmentManager.Read(null);
            addEmployeeViewModel.Posts = postManager.Read(null);
            return View(addEmployeeViewModel);
        }
        [HttpPost]
        public ActionResult Add(AddEmployeeViewModel addEmployeeViewModel)
        {
            addEmployeeViewModel.Employee.Avatar = new ArchiveFile();

            if (addEmployeeViewModel.FileUpload != null && addEmployeeViewModel.FileUpload.ContentLength > 0 && addEmployeeViewModel.FileUpload.ContentType.Contains("image"))
            {
                //Get Content of image
                MemoryStream target = new MemoryStream();
                addEmployeeViewModel.FileUpload.InputStream.CopyTo(target);
                addEmployeeViewModel.Employee.Avatar.Content = target.ToArray();
                addEmployeeViewModel.Employee.Avatar.Name = addEmployeeViewModel.FileUpload.FileName;
                addEmployeeViewModel.Employee.Avatar.ContentType = addEmployeeViewModel.FileUpload.ContentType;
            }


            if (!string.IsNullOrEmpty(addEmployeeViewModel.BirthDay))
            {
                addEmployeeViewModel.Employee.BirthDay = addEmployeeViewModel.BirthDay.toEnglishNumber().ToGeorgianDateTime();
            }

            EmployeeManager employeeManager = new EmployeeManager();
            //Add posts and departments to employee
            employeeManager.AddPostDepartment(addEmployeeViewModel.Employee, addEmployeeViewModel.SelectedDepartments, addEmployeeViewModel.SelectedPosts);
            employeeManager.Add(addEmployeeViewModel.Employee);
            employeeManager.saveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            if (id != null && id!=Guid.Empty)
            {
                EmployeeManager employeeManager = new EmployeeManager();
                EditEmployeeViewModel employeeViewModel = new EditEmployeeViewModel();
                DepartmentManager departmentManager = new DepartmentManager();
                PostManager postManager = new PostManager();
                employeeViewModel.PageId = "ShowEmployee";
                employeeViewModel.Employee = employeeManager.Read((Guid)id);
                if (employeeViewModel.Employee == null)
                    return RedirectToAction("Index");
                employeeViewModel.Departments = departmentManager.Read(null);
                employeeViewModel.Posts = postManager.Read(null);
                return View(employeeViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditEmployeeViewModel editEmployeeViewModel)
        {
            EmployeeManager employeeManager = new EmployeeManager();
            //List<Guid> GuidList = new List<Guid>();
            if (editEmployeeViewModel.Employee.Avatar == null)
            {
                editEmployeeViewModel.Employee.Avatar = new ArchiveFile();
            }
            if (editEmployeeViewModel.fileUpload != null && editEmployeeViewModel.fileUpload.ContentLength > 0 && editEmployeeViewModel.fileUpload.ContentType.Contains("image"))
            {
                //Get Content of image

                MemoryStream target = new MemoryStream();
                editEmployeeViewModel.fileUpload.InputStream.CopyTo(target);
                editEmployeeViewModel.Employee.Avatar.Content = target.ToArray();
                editEmployeeViewModel.Employee.Avatar.Name = editEmployeeViewModel.fileUpload.FileName;
                editEmployeeViewModel.Employee.Avatar.ContentType = editEmployeeViewModel.fileUpload.ContentType;
            }
            else if (!editEmployeeViewModel.Flag)
            {
                EmployeeManager otherManager = new EmployeeManager();
                Employee oldEmployee = otherManager.Read(editEmployeeViewModel.Employee.Id);
                otherManager.Dispose();
                editEmployeeViewModel.Employee.Avatar.Content = oldEmployee.Avatar.Content;
                editEmployeeViewModel.Employee.Avatar.Name = oldEmployee.Avatar.Name;
                editEmployeeViewModel.Employee.Avatar.ContentType = oldEmployee.Avatar.ContentType;
            }

            if (!string.IsNullOrEmpty(editEmployeeViewModel.BirthDay))
            {
                editEmployeeViewModel.Employee.BirthDay = editEmployeeViewModel.BirthDay.toEnglishNumber().ToGeorgianDateTime();
            }

            employeeManager.Update(editEmployeeViewModel.Employee);
            employeeManager.EditPostDepartment(editEmployeeViewModel.Employee.Id, editEmployeeViewModel.SelectedDepartments, editEmployeeViewModel.SelectedPosts);
            employeeManager.saveChanges();

            return RedirectToAction("Index");
        }
    }
}