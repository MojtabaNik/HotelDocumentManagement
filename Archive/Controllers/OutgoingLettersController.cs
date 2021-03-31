using BusinessModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Archive.ViewModel;
using System.Text;
using EntityModel;
using System.IO;
using Archive.ViewModel.OutgoingLetters;
using Aspose.Pdf;
using Aspose.Words.Reporting;
using Aspose.Words.Saving;
using DBProvider;
using Microsoft.Ajax.Utilities;
using Microsoft.ApplicationInsights.DataContracts;
using Document = Aspose.Words.Document;
using SaveFormat = Aspose.Words.SaveFormat;

namespace Archive.Controllers
{
    public class OutgoingLettersController : Controller
    {
        UserManager userManager = new UserManager();
        private User cUser;
        private bool isEdit = false;
        // GET: Outgoing letters Json Methods
        public ActionResult GetPersons(List<Guid> idList, Guid? postGuid, bool noPost)
        {
            if (idList != null && idList.Count > 0)
            {
                if (!noPost)
                {
                    UserManager usermanager = new UserManager();
                    OrganizationManager organizationManager = new OrganizationManager();
                    List<Organization> checkedOrganizations = new List<Organization>();
                    User currentUser = usermanager.Read(new Guid(User.Identity.Name));
                    foreach (var id in idList)
                    {
                        checkedOrganizations.Add(organizationManager.Read(id));
                        //checkedOrganizations.AddRange(organizationManager.GetChildItems(id, new List<Organization>()));
                    }
                    PersonManager personManager = new PersonManager();
                    List<Person> peopleList = new List<Person>();
                    //Remove non organized people from json
                    //peopleList.AddRange(usermanager.GetUserShowablePeopleList(usermanager.Read(new Guid(User.Identity.Name))));
                    foreach (var organization in checkedOrganizations.Distinct())
                    {
                        peopleList.AddRange(personManager.GetOrganizationPersons(organization, postGuid));
                    }

                    return Json(peopleList.Distinct().ToList().Select(x => new { x.Id, x.FirstName, x.LastName }));
                }
                else
                {
                    UserManager usermanager = new UserManager();
                    OrganizationManager organizationManager = new OrganizationManager();
                    List<Organization> checkedOrganizations = new List<Organization>();
                    User currentUser = usermanager.Read(new Guid(User.Identity.Name));
                    PostManager postManager = new PostManager();
                    foreach (var id in idList)
                    {
                        checkedOrganizations.Add(organizationManager.Read(id));
                        checkedOrganizations.AddRange(organizationManager.GetChildItems(id, new List<Organization>()));
                    }
                    PersonManager personManager = new PersonManager();

                    List<Tuple<Person, Post, Organization>> peopleList = (from organization in checkedOrganizations.Distinct() from item in personManager.GetOrganizationPersons(organization) select new Tuple<Person, Post, Organization>(item.Item1, item.Item2, item.Item3)).ToList();

                    return Json(peopleList.Select(x => new { x.Item1.Id, x.Item1.FirstName, x.Item1.LastName, x.Item2.Name, postID = x.Item2.Id, organizationName = x.Item3.GetFullOrganizationPath(), organizationID = x.Item3.Id }));
                }
            }
            else
            {
                UserManager usermanager = new UserManager();
                return Json(usermanager.GetUserShowablePeopleList(usermanager.Read(new Guid(User.Identity.Name))).Select(x => new { x.Id, x.FirstName, x.LastName }));
            }
        }
        public ActionResult GetEmployees(List<Guid> idList, Guid? postGuid, bool noPost)
        {
            if (idList != null && idList.Count > 0)
            {
                if (!noPost)
                {

                    DepartmentManager departmentManager = new DepartmentManager();
                    List<Department> checkedDepartments = new List<Department>();
                    foreach (var id in idList)
                    {
                        checkedDepartments.Add(departmentManager.Read(id));
                        //checkedOrganizations.AddRange(organizationManager.GetChildItems(id, new List<Organization>()));
                    }
                    EmployeeManager employeeManager = new EmployeeManager();
                    List<Employee> employeeList = new List<Employee>();
                    //Remove non organized people from json
                    //peopleList.AddRange(usermanager.GetUserShowablePeopleList(usermanager.Read(new Guid(User.Identity.Name))));
                    foreach (var department in checkedDepartments.Distinct())
                    {
                        employeeList.AddRange(employeeManager.GetDepartmentEmployees(department, postGuid));
                    }

                    return Json(employeeList.Distinct().ToList().Select(x => new { x.Id, x.FirstName, x.LastName }));
                }
                else
                {
                    DepartmentManager departmentManager = new DepartmentManager();
                    List<Department> checkedDepartments = new List<Department>();
                    foreach (var id in idList)
                    {
                        checkedDepartments.Add(departmentManager.Read(id));
                        checkedDepartments.AddRange(departmentManager.GetChildItems(id, new List<Department>()));
                    }
                    EmployeeManager employeeManager = new EmployeeManager();

                    List<Tuple<Employee, Post, Department>> peopleList = (from department in checkedDepartments.Distinct() from item in employeeManager.GetDepartmentEmployees(department) select new Tuple<Employee, Post, Department>(item.Item1, item.Item2, item.Item3)).ToList();

                    return Json(peopleList.Select(x => new { x.Item1.Id, x.Item1.FirstName, x.Item1.LastName, x.Item2.Name, postID = x.Item2.Id, departmentName = x.Item3.GetFullDepartmentPath(), departmentID = x.Item3.Id }));
                }
            }
            else
            {

                EmployeeManager employeeManager = new EmployeeManager();
                return Json(employeeManager.NoneDepartment().Select(x => new { x.Id, x.FirstName, x.LastName }));
            }
        }
        //End Json Methods

        // GET: IncomingLetters
        public ActionResult Index(string Case = null, string Content = null)
        {

            OutgoingLettersViewModel outgoingLettersViewModel = new OutgoingLettersViewModel();
            SendLetterManager sendLetterManager = new SendLetterManager();
            outgoingLettersViewModel.PageId = "ShowOutgoingLetters";
            StringBuilder stb = new StringBuilder();
            stb.Append("");
            if (Case != null)
                stb.Append($"Case == {Case} &&");
            if (Content != null)
                stb.Append($"Content == {Content} &&");
            stb.Append("1==1");
            outgoingLettersViewModel.ListSendLetters = sendLetterManager.Read(stb.ToString(), "ParentSendLetter,ParentRecievedLetterOut").ToList();
            return View(outgoingLettersViewModel);
        }


        private string getWhileLoopDataOrganization()
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
        private string getWhileLoopDataDepartment()
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
                GetChildItems(departmentManager.Read(null).First().Id, new List<Department>());
                //End tag
                htmlStr2.Append("]");
            }

            htmlStr2.Append("}]");
            return htmlStr2.ToString();
        }
        private StringBuilder htmlStr = new System.Text.StringBuilder();
        private StringBuilder htmlStr2 = new System.Text.StringBuilder();
        private List<Organization> GetChildItems(Guid parentId, List<Organization> EmptyRole)
        {


            if (GetRowsByParent(parentId).Count > 0) // If this item have childs then i print them
            {
                foreach (Organization row in GetRowsByParent(parentId))
                {
                    EmptyRole.Add(row);

                    htmlStr.Append("{id:\"");
                    htmlStr.Append(row.Id);
                    htmlStr.Append("\",text: \"");
                    htmlStr.Append(row.Name);
                    htmlStr.Append("\",expanded: true,spriteCssClass: \"folder\"");
                    if (GetRowsByParent(row.Id).Count > 0)
                    {
                        htmlStr.Append(",items: [");
                        GetChildItems(row.Id, EmptyRole); // Search for childs that belong to this item
                        htmlStr.Append("]");
                    }
                    htmlStr.Append("},");
                }
                htmlStr.Length--;
            }
            return EmptyRole;
        }
        private List<Department> GetChildItems(Guid parentId, List<Department> EmptyRole)
        {


            if (GetRowsByParentDep(parentId).Count > 0) // If this item have childs then i print them
            {
                foreach (Department row in GetRowsByParentDep(parentId))
                {
                    EmptyRole.Add(row);

                    htmlStr2.Append("{id:\"");
                    htmlStr2.Append(row.Id);
                    htmlStr2.Append("\",text: \"");
                    htmlStr2.Append(row.Name);
                    htmlStr2.Append("\",expanded: true,spriteCssClass: \"folder\"");
                    if (GetRowsByParentDep(row.Id).Count > 0)
                    {
                        htmlStr2.Append(",items: [");
                        GetChildItems(row.Id, EmptyRole); // Search for childs that belong to this item
                        htmlStr2.Append("]");
                    }
                    htmlStr2.Append("},");
                }
                htmlStr2.Length--;
            }
            return EmptyRole;
        }
        private List<Organization> GetRowsByParent(Guid parentId)
        {
            OrganizationManager organizationManager = new OrganizationManager();
            List<Organization> DbMenuTable = new List<Organization>();
            if (!isEdit)
                DbMenuTable = userManager.GetUserShowableOrganizations(cUser);
            else
            {
                DbMenuTable = organizationManager.Read(null);
            }
            List<Organization> r = new List<Organization>();
            foreach (var item in DbMenuTable)
            {
                if (item.ParentOrganization?.Id == parentId)
                    r.Add(item);
            }

            return r;
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
        [HttpGet]
        public ActionResult Add()
        {
            AddOutgoingViewModel outgoingLettersViewModel = new AddOutgoingViewModel { PageId = "CreateLetter" };
            EmployeeManager employeeManager = new EmployeeManager();
            CaseManager caseManager = new CaseManager();
            TemplateManager templateManager = new TemplateManager();
            PostManager postManager = new PostManager();

            User currentUser = userManager.Read(new Guid(User.Identity.Name));
            cUser = currentUser;
            //outgoingLettersViewModel.Departments = departmentManager.Read(null);
            //
            outgoingLettersViewModel.DepartmentJson = getWhileLoopDataDepartment();

            //outgoingLettersViewModel.Organizations = organizationManager.Read(null);
            //outgoingLettersViewModel.FilterOrganizations = currentUser.FilterOrganizations.ToList();

            //
            outgoingLettersViewModel.OrganizationJson = getWhileLoopDataOrganization();

            //
            outgoingLettersViewModel.Posts = postManager.Read(null);
            //
            outgoingLettersViewModel.People = userManager.GetUserShowablePeopleList(currentUser);
            //outgoingLettersViewModel.FilterPersons = currentUser.FilterPeople.ToList();

            //
            outgoingLettersViewModel.Employees = employeeManager.NoneDepartment();
            //
            outgoingLettersViewModel.Cases = caseManager.Read(null);
            //
            outgoingLettersViewModel.Templates = templateManager.Read(null);

            //outgoingLettersViewModel.OrganizationFilterString = string.Join(",", currentUser.FilterOrganizations.Select(x => x.Id.ToString()));
            //outgoingLettersViewModel.PersonFilterString = string.Join(",", currentUser.FilterPeople.Select(x => x.Id.ToString()));
            //outgoingLettersViewModel.DepartmentString = string.Join(",", outgoingLettersViewModel.Departments.Select(x => x.Id.ToString()));
            return View(outgoingLettersViewModel);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Add(AddOutgoingViewModel outgoingLettersViewModel)
        {
            SendLetterManager sendLetterManager = new SendLetterManager();
            //Add letter files
            if (outgoingLettersViewModel.fileUploadLetters != null && outgoingLettersViewModel.fileUploadLetters.Count > 0)
            {

                outgoingLettersViewModel.sendLetter.sendLetterFiles = new List<SendLetterFile>(); //

                foreach (var item in outgoingLettersViewModel.fileUploadLetters)
                {
                    if (item == null || item.ContentLength <= 0) continue;
                    var target = new MemoryStream();
                    SendLetterFile sendLetterFile = new SendLetterFile();
                    item.InputStream.CopyTo(target);
                    sendLetterFile.File = new ArchiveFile
                    {
                        Content = target.ToArray(),
                        Name = item.FileName,
                        ContentType = item.ContentType
                    };
                    outgoingLettersViewModel.sendLetter.sendLetterFiles.Add(sendLetterFile);
                }

            }

            //Add letter appendages
            if (outgoingLettersViewModel.fileUploadAppendages != null && outgoingLettersViewModel.fileUploadAppendages.Count > 0)
            {
                outgoingLettersViewModel.sendLetter.Appendages = new List<SendAppendage>();
                foreach (var item in outgoingLettersViewModel.fileUploadAppendages)
                {
                    if (item == null || item.ContentLength <= 0) continue;
                    var target = new MemoryStream();
                    var sendAppendage = new SendAppendage();
                    item.InputStream.CopyTo(target);
                    sendAppendage.File = new ArchiveFile
                    {
                        Content = target.ToArray(),
                        Name = item.FileName,
                        ContentType = item.ContentType
                    };
                    outgoingLettersViewModel.sendLetter.Appendages.Add(sendAppendage);
                }
            }

            //Date Times
            if (!string.IsNullOrEmpty(outgoingLettersViewModel.LetterDateTime))
            {
                outgoingLettersViewModel.sendLetter.LetterDateTime =
                    outgoingLettersViewModel.LetterDateTime.ToGeorgianDateTime();
            }
            outgoingLettersViewModel.sendLetter.ArchiveDateTime = DateTime.Now;
            outgoingLettersViewModel.sendLetter.LastModifyDateTime = DateTime.Now;
            //outgoingLettersViewModel.sendLetter.User = Sessions.CurrentUser;
            sendLetterManager.AddUser(outgoingLettersViewModel.sendLetter, new Guid(User.Identity.Name));


            //Set Parent Letter
            sendLetterManager.AddParent(outgoingLettersViewModel.sendLetter, outgoingLettersViewModel.ParentletterId);
            //Add organization , post and person
            if (outgoingLettersViewModel.PersonGuid != Guid.Empty || outgoingLettersViewModel.OrganizationGuid != Guid.Empty)
                sendLetterManager.AddReciever(outgoingLettersViewModel.sendLetter, outgoingLettersViewModel.OrganizationGuid, outgoingLettersViewModel.PersonGuid, outgoingLettersViewModel.PostGuid);
            //Add department , post and employee
            if (outgoingLettersViewModel.EmployeeGuid != Guid.Empty || outgoingLettersViewModel.DepartmentGuid != Guid.Empty)
                sendLetterManager.AddSenderEmployee(outgoingLettersViewModel.sendLetter, outgoingLettersViewModel.DepartmentGuid, outgoingLettersViewModel.EmployeeGuid, outgoingLettersViewModel.DepartmentPostGuid);

            //set rowNumber Logic
            outgoingLettersViewModel.sendLetter.RowNumber = outgoingLettersViewModel.autoRowNumber ? sendLetterManager.AutoRowNumber(outgoingLettersViewModel.sendLetter.SenderEmployee?.Department) : outgoingLettersViewModel.RowNumber;

            //Add cases
            if (outgoingLettersViewModel.SelectedCasesGuid != null && outgoingLettersViewModel.SelectedCasesGuid.Count > 0)
                sendLetterManager.AddCases(outgoingLettersViewModel.sendLetter, outgoingLettersViewModel.SelectedCasesGuid);


            //Start Processing Files and Appendages to Save them as pdf
            sendLetterManager.GeneratePDF(outgoingLettersViewModel.sendLetter, outgoingLettersViewModel.LetterTemplate);
            sendLetterManager.Add(outgoingLettersViewModel.sendLetter);
            sendLetterManager.saveChanges();




            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid? ID)
        {
            isEdit = true;
            EditOutgoingLettersViewModel editOutgoingLettersViewModel = new EditOutgoingLettersViewModel();

            SendLetterManager sendLetterManager = new SendLetterManager();
            PostManager postManager = new PostManager();
            PersonManager personManager = new PersonManager();
            CaseManager caseManager = new CaseManager();
            EmployeeManager employeeManager = new EmployeeManager();

            TemplateManager templateManager = new TemplateManager();

            User currentUser = userManager.Read(new Guid(User.Identity.Name));
            cUser = currentUser;


            if (ID != null && ID != Guid.Empty)
            {
                editOutgoingLettersViewModel.sendLetter = sendLetterManager.Read((Guid)ID);

                if (editOutgoingLettersViewModel.sendLetter == null)
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index");


            //Selected Person and Organization and Post Logic

            if (editOutgoingLettersViewModel.sendLetter.Recievers != null && editOutgoingLettersViewModel.sendLetter.Recievers.Count>0 && editOutgoingLettersViewModel.sendLetter.Recievers.First() != null)
            {
                if (editOutgoingLettersViewModel.sendLetter.Recievers.First().Organization != null)
                {
                    editOutgoingLettersViewModel.People = personManager.GetOrganizationPersons(editOutgoingLettersViewModel.sendLetter.Recievers.First().Organization, editOutgoingLettersViewModel.sendLetter.Recievers.First().Post?.Id ?? new Guid());
                }
                else
                {
                    editOutgoingLettersViewModel.People = userManager.GetUserShowablePeopleList(currentUser);
                }

                //Fix problem of not showable but selected Person
                if (editOutgoingLettersViewModel.sendLetter.Recievers.First().Person != null)
                    if (editOutgoingLettersViewModel.People.All(x => x.Id != editOutgoingLettersViewModel.sendLetter.Recievers.First().Person.Id))
                    {
                        editOutgoingLettersViewModel.People.Add(editOutgoingLettersViewModel.sendLetter.Recievers.First().Person);
                    }
            }
            else
            {
                editOutgoingLettersViewModel.People = userManager.GetUserShowablePeopleList(currentUser);
            }


            editOutgoingLettersViewModel.OrganizationJson = getWhileLoopDataOrganization();


            //Selected Employee and Department and Post Logic

            if (editOutgoingLettersViewModel.sendLetter.SenderEmployee != null)
            {
                if (editOutgoingLettersViewModel.sendLetter.SenderEmployee.Department != null)
                {
                    editOutgoingLettersViewModel.Employees = employeeManager.GetDepartmentEmployees(editOutgoingLettersViewModel.sendLetter.SenderEmployee.Department, editOutgoingLettersViewModel.sendLetter.SenderEmployee.Post?.Id ?? new Guid());
                }
                else
                {
                    editOutgoingLettersViewModel.Employees = employeeManager.NoneDepartment();
                }
            }
            else
            {
                editOutgoingLettersViewModel.Employees = employeeManager.NoneDepartment();
            }

            editOutgoingLettersViewModel.DepartmentJson = getWhileLoopDataDepartment();



            editOutgoingLettersViewModel.Cases = caseManager.Read(null);
            editOutgoingLettersViewModel.Posts = postManager.Read(null);
            editOutgoingLettersViewModel.Templates = templateManager.Read(null);


            editOutgoingLettersViewModel.LetterFiles = new List<FileViewModel>();
            foreach (var itemFile in editOutgoingLettersViewModel.sendLetter.sendLetterFiles)
            {
                FileViewModel fileViewModel = new FileViewModel
                {
                    Content = itemFile.File.Content.GetContentBase64(itemFile.File.ContentType),
                    Id = itemFile.Id
                };
                editOutgoingLettersViewModel.LetterFiles.Add(fileViewModel);
            }

            editOutgoingLettersViewModel.LetterAppendages = new List<FileViewModel>();
            foreach (var itemFile in editOutgoingLettersViewModel.sendLetter.Appendages)
            {
                FileViewModel fileViewModel = new FileViewModel
                {
                    Content = itemFile.File.Content.GetContentBase64(itemFile.File.ContentType),
                    Id = itemFile.Id
                };
                editOutgoingLettersViewModel.LetterAppendages.Add(fileViewModel);
            }


            //Process Letter Parent
            //Set Parent Letter
            try
            {
                switch (editOutgoingLettersViewModel.sendLetter.ParentType)
                {
                    case ParentType.Send:
                        var parent = sendLetterManager.GetParent(editOutgoingLettersViewModel.sendLetter.Id);
                        editOutgoingLettersViewModel.sendLetter.ParentSendLetter = parent;

                        break;
                    case ParentType.Receive:
                        var parent2 = sendLetterManager.GetParentOut(editOutgoingLettersViewModel.sendLetter.Id);
                        editOutgoingLettersViewModel.sendLetter.ParentReceivedLetterOut = parent2;

                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
            return View(editOutgoingLettersViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditOutgoingLettersViewModel editOutgoingLettersViewModel)
        {
            SendLetterManager sendLetterManager = new SendLetterManager();
            SendLetter oldLetter = sendLetterManager.Read(editOutgoingLettersViewModel.sendLetter.Id);

            #region temp

            TempSendLetterManager tempSendLetterManager = new TempSendLetterManager();
            //Get Letter before Edit
            SendLetter sendletter = tempSendLetterManager.ReturnReceivedLetter(editOutgoingLettersViewModel.sendLetter.Id);
            if (sendletter == null)
            {
                throw new Exception("نامه مورد نظر یافت نگردید.");
            }
            TempSendLetter tempSendLetter = new TempSendLetter
            {
                User = sendletter.User,
                Recievers = sendletter?.Recievers ?? null,
                ParentType = sendletter.ParentType,
                AppendageContent = sendletter.AppendageContent,
                LetterFileContent = sendletter.LetterFileContent,
                ArchiveDateTime = sendletter.ArchiveDateTime,
                LetterDateTime = sendletter.LetterDateTime,
                ModifyDateTime = sendletter.LastModifyDateTime,
                Assortment = sendletter.Assortment,
                Cases = sendletter.Cases,
                RowNumber = sendletter.RowNumber,
                Description = sendletter.Description,
                SendLetter = sendletter,
                ParentReceivedLetterOutTemp = sendletter.ParentReceivedLetterOut,
                ParentSendLetter = sendletter.ParentSendLetter,
                TempSendLetterFiles = new List<TempSendLetterFile>(),
                Appendages = new List<TempSendAppendage>(),
                LetterFileAppendageContent = sendletter.LetterFileAppendageContent,
                Active = true,
                SenderEmployee = sendletter.SenderEmployee
            };
            //Temp Collection of Cases
            //foreach (var item in sendletter.Cases)
            //{
            //    tempSendLetter.Cases.Add(item);
            //}
            //Copy files to temp
            foreach (var item in sendletter.sendLetterFiles)
            {
                TempSendLetterFile tempSendLetterFile = new TempSendLetterFile { File = item.File };
                tempSendLetter.TempSendLetterFiles.Add(tempSendLetterFile);
            }
            //Copy Appendages to temp
            foreach (var item in sendletter.Appendages)
            {
                TempSendAppendage tempSendAppendage = new TempSendAppendage { File = item.File };
                tempSendLetter.Appendages.Add(tempSendAppendage);
            }
            //Copy Recievers
            //foreach (var item in sendletter.Recievers)
            //{
            //    Reciever r = new Reciever();
            //    r.Id = item.Id;
            //    r.Organization = item.Organization;
            //    r.Person = item.Person;
            //    r.Post = item.Post;
            //}

            tempSendLetterManager.Add(tempSendLetter);
            tempSendLetterManager.saveChanges();

            #endregion
            //Set assortments
            oldLetter.Assortment = editOutgoingLettersViewModel.sendLetter.Assortment;
            //Set Description
            oldLetter.Description = editOutgoingLettersViewModel.sendLetter.Description;
            //Set letter content 
            oldLetter.Body = editOutgoingLettersViewModel.sendLetter.Body;
            //Set Letter DateTime
            if (!string.IsNullOrEmpty(editOutgoingLettersViewModel.LetterDateTime))
            {
                oldLetter.LetterDateTime = editOutgoingLettersViewModel.LetterDateTime.toEnglishNumber().ToGeorgianDateTime();
            }

            oldLetter.LastModifyDateTime = DateTime.Now;

            if (oldLetter.User.Id != new Guid(User.Identity.Name))
            {
                sendLetterManager.AddUser(oldLetter, new Guid(User.Identity.Name));
            }

            //upload files
            sendLetterManager.UpdateSendLetterFiles(oldLetter, editOutgoingLettersViewModel.fileUploadLetters, editOutgoingLettersViewModel.FilesID);
            sendLetterManager.UpdateSendLetterAppendages(oldLetter, editOutgoingLettersViewModel.fileUploadAppendages, editOutgoingLettersViewModel.AppendagesID);


            oldLetter.ParentType = editOutgoingLettersViewModel.sendLetter.ParentType;
            //Set Parent Letter
            sendLetterManager.AddParent(oldLetter, editOutgoingLettersViewModel.ParentletterId);
            //update organization , post and person
            if (editOutgoingLettersViewModel.PersonGuid != Guid.Empty || editOutgoingLettersViewModel.OrganizationGuid != Guid.Empty)
                sendLetterManager.EditReciever(oldLetter, editOutgoingLettersViewModel.OrganizationGuid, editOutgoingLettersViewModel.PersonGuid, editOutgoingLettersViewModel.PostGuid);
            //update department , post and employee
            if (editOutgoingLettersViewModel.EmployeeGuid != Guid.Empty || editOutgoingLettersViewModel.DepartmentGuid != Guid.Empty)
                sendLetterManager.EditSenderEmployee(oldLetter, editOutgoingLettersViewModel.DepartmentGuid, editOutgoingLettersViewModel.EmployeeGuid, editOutgoingLettersViewModel.DepartmentPostGuid);

            //set rowNumber Logic
            oldLetter.RowNumber = editOutgoingLettersViewModel.autoRowNumber ? sendLetterManager.AutoRowNumber(oldLetter.SenderEmployee?.Department) : editOutgoingLettersViewModel.RowNumber;

            //Add cases
                sendLetterManager.EditCases(oldLetter, editOutgoingLettersViewModel.SelectedCasesGuid);


            //Start Processing Files and Appendages to Save them as pdf
            sendLetterManager.GeneratePDF(oldLetter, editOutgoingLettersViewModel.LetterTemplate);
            sendLetterManager.Update(oldLetter);
            sendLetterManager.saveChanges();
            return RedirectToAction("Index");
        }


        public FileResult Download(Guid id)
        {
            SendLetterManager sendLetterManager = new SendLetterManager();
            SendLetter sendLetter = new SendLetter();
            sendLetter = sendLetterManager.Read(id);
            byte[] file = sendLetter?.LetterFileContent;
            if (file != null && file.Length > 0)
            {
                string fileName = id + ".pdf";
                return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                return null;
            }
        }

        public FileResult DownloadFile(Guid id)
        {
            SendLetterFileManager sendLetterFileManagere = new SendLetterFileManager();
            ArchiveFile file = sendLetterFileManagere.Read(id)?.File;
            if (file != null)
            {
                byte[] fileBytes = file.Content;
                string fileName = file.Name;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                return null;
            }
        }
        public FileResult DownloadAppendage(Guid id)
        {
            SendLetterAppendageManager sendLetterAppendageManager = new SendLetterAppendageManager();
            ArchiveFile file = sendLetterAppendageManager.Read(id)?.File;
            if (file != null)
            {
                byte[] fileBytes = file.Content;
                string fileName = file.Name;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                return null;
            }
        }
    }
}