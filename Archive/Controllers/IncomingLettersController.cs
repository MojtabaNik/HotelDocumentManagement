using BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Archive.ViewModel;
using System.Text;
using EntityModel;
using System.IO;
using Archive.ViewModel.Templates;
using DBProvider;
using System.Drawing;
using System.IO.Pipes;
using Archive.ViewModel.IncomingLeters;

namespace Archive.Controllers
{

    public class IncomingLettersController : Controller
    {
        UserManager userManager = new UserManager();
        private User cUser;
        private bool isEdit = false;
        // GET: IncomingLetters
        public ActionResult GetPersons(List<Guid> idList, Guid? postGuid, bool noPost)
        {
            if (idList != null && idList.Count > 0)
            {
                if (!noPost)
                {
                    OrganizationManager organizationManager = new OrganizationManager();
                    List<Organization> checkedOrganizations = new List<Organization>();
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
                    OrganizationManager organizationManager = new OrganizationManager();
                    List<Organization> checkedOrganizations = new List<Organization>();
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
        public ActionResult Index(string Case = null, string Content = null)
        {
            ReceiveLetterManager receiveLetterManager = new ReceiveLetterManager();
            IncomingLettersViewModel incomingLettersViewModel = new IncomingLettersViewModel
            {
                PageId = "IncomingShowAll"
            };
            StringBuilder stb = new StringBuilder();

            stb.Append("");

            if (Case != null)
                stb.Append($"Case == {Case} &&");

            if (Content != null)
                stb.Append($"Content == {Content} &&");

            stb.Append("1==1");

            incomingLettersViewModel.ListReciveLetters = receiveLetterManager.Read(stb.ToString(), "ParentReceiveLetter,ParentSendLetterOut").ToList();
            return View(incomingLettersViewModel);
        }
        private StringBuilder htmlStr = new System.Text.StringBuilder();
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
        [HttpGet]
        public ActionResult Add()
        {
            AddIncomingLettersViewModel incomingLettersViewModel = new AddIncomingLettersViewModel() { PageId = "IncomingArchive" };
            OrganizationManager organizationManager = new OrganizationManager();
            //DepartmentManager departmentManager = new DepartmentManager();
            PersonManager personManager = new PersonManager();
            PostManager postManager = new PostManager();
            CaseManager caseManager = new CaseManager();
            User currentUser = userManager.Read(new Guid(User.Identity.Name));
            cUser = currentUser;

            //incomingLettersViewModel.Organizations = organizationManager.Read(null);
            //incomingLettersViewModel.FilterOrganizations = currentUser.FilterOrganizations.ToList();
            incomingLettersViewModel.OrganizationJson = getWhileLoopData();

            incomingLettersViewModel.People = userManager.GetUserShowablePeopleList(currentUser);
            //incomingLettersViewModel.FilterPersons = currentUser.FilterPeople.ToList();
            incomingLettersViewModel.Cases = caseManager.Read(null);

            incomingLettersViewModel.Posts = postManager.Read(null);

            //incomingLettersViewModel.OrganizationFilterString = string.Join(",", currentUser.FilterOrganizations.Select(x => x.Id.ToString()));
            //incomingLettersViewModel.PersonFilterString = string.Join(",", currentUser.FilterPeople.Select(x => x.Id.ToString()));
            //incomingLettersViewModel.receivedLetter = new ReceivedLetter();
            return View(incomingLettersViewModel);
        }

        [HttpPost]
        public ActionResult Add(AddIncomingLettersViewModel incomingLettersViewModel)
        {
            ReceiveLetterManager receiveLetterManager = new ReceiveLetterManager();
            //Add letter files
            if (incomingLettersViewModel.FileUploadLetters != null && incomingLettersViewModel.FileUploadLetters.Count > 0)
            {
                incomingLettersViewModel.receivedLetter.ReceiveLetterFiles = new List<ReceiveLetterFile>();

                foreach (var item in incomingLettersViewModel.FileUploadLetters)
                {
                    if (item == null || item.ContentLength <= 0) continue;
                    var target = new MemoryStream();
                    ReceiveLetterFile receiveLetterFile = new ReceiveLetterFile();
                    item.InputStream.CopyTo(target);
                    receiveLetterFile.File = new ArchiveFile
                    {
                        Content = target.ToArray(),
                        Name = item.FileName,
                        ContentType = item.ContentType
                    };
                    incomingLettersViewModel.receivedLetter.ReceiveLetterFiles.Add(receiveLetterFile);
                }
            }
            //Add letter appendages
            if (incomingLettersViewModel.FileUploadAppendages != null && incomingLettersViewModel.FileUploadAppendages.Count > 0)
            {
                incomingLettersViewModel.receivedLetter.Appendages = new List<ReceiveAppendage>();
                foreach (var item in incomingLettersViewModel.FileUploadAppendages)
                {
                    if (item != null && item.ContentLength > 0)
                    {
                        var target = new MemoryStream();
                        ReceiveAppendage receiveAppendage = new ReceiveAppendage();
                        item.InputStream.CopyTo(target);
                        receiveAppendage.File = new ArchiveFile
                        {
                            Content = target.ToArray(),
                            Name = item.FileName,
                            ContentType = item.ContentType
                        };
                        incomingLettersViewModel.receivedLetter.Appendages.Add(receiveAppendage);
                    }
                }
            }
            //Date Times
            if (!string.IsNullOrEmpty(incomingLettersViewModel.LetterDateTime))
            {
                incomingLettersViewModel.receivedLetter.LetterDateTime =
                    incomingLettersViewModel.LetterDateTime.ToGeorgianDateTime();
            }
            incomingLettersViewModel.receivedLetter.ArchiveDateTime = DateTime.Now;
            incomingLettersViewModel.receivedLetter.LastModifyDateTime = DateTime.Now;
            //Row number and user
            incomingLettersViewModel.receivedLetter.RowNumber = incomingLettersViewModel.RowNumber;
            receiveLetterManager.AddUser(incomingLettersViewModel.receivedLetter, new Guid(User.Identity.Name));

            //Set Parent Letter
            receiveLetterManager.SetParent(incomingLettersViewModel.receivedLetter, incomingLettersViewModel.ParentletterId);


            //Add organization , post and person
            if (incomingLettersViewModel.PersonGuid != Guid.Empty || incomingLettersViewModel.OrganizationGuid != Guid.Empty)
                receiveLetterManager.AddSender(incomingLettersViewModel.receivedLetter, incomingLettersViewModel.OrganizationGuid, incomingLettersViewModel.PersonGuid, incomingLettersViewModel.PostGuid);


            //Add cases
            if (incomingLettersViewModel.SelectedCasesGuid != null && incomingLettersViewModel.SelectedCasesGuid.Count > 0)
                receiveLetterManager.AddCases(incomingLettersViewModel.receivedLetter, incomingLettersViewModel.SelectedCasesGuid);
            //Add 3 pdf to db
            receiveLetterManager.GeneratePDF(incomingLettersViewModel.receivedLetter);

            //Add letter to db and save changes
            receiveLetterManager.Add(incomingLettersViewModel.receivedLetter);
            receiveLetterManager.saveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid? ID)
        {
            isEdit = true;
            EditIncomingLettersViewModel editIncomingLettersViewModel = new EditIncomingLettersViewModel();

            ReceiveLetterManager receiveLetterManager = new ReceiveLetterManager();
            PostManager postManager = new PostManager();
            PersonManager personManager = new PersonManager();
            CaseManager caseManager = new CaseManager();
            User currentUser = userManager.Read(new Guid(User.Identity.Name));
            cUser = currentUser;

            if (ID != null && ID != Guid.Empty)
            {
                editIncomingLettersViewModel.ReceivedLetter = receiveLetterManager.Read((Guid)ID);

                if (editIncomingLettersViewModel.ReceivedLetter == null)
                    return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index");


            //Selected Person and Organization and Post Logic

            if (editIncomingLettersViewModel.ReceivedLetter.Sender != null)
            {
                if (editIncomingLettersViewModel.ReceivedLetter.Sender.Organization != null)
                {
                    editIncomingLettersViewModel.People = personManager.GetOrganizationPersons(editIncomingLettersViewModel.ReceivedLetter.Sender.Organization, editIncomingLettersViewModel.ReceivedLetter.Sender.Post?.Id ?? new Guid());
                }
                else
                {
                    editIncomingLettersViewModel.People = userManager.GetUserShowablePeopleList(currentUser);
                }

                //Fix problem of not showable but selected Person
                if (editIncomingLettersViewModel.ReceivedLetter.Sender.Person != null)
                    if (editIncomingLettersViewModel.People.All(x => x.Id != editIncomingLettersViewModel.ReceivedLetter.Sender.Person.Id))
                    {
                        editIncomingLettersViewModel.People.Add(editIncomingLettersViewModel.ReceivedLetter.Sender.Person);
                    }
            }
            else
            {
                editIncomingLettersViewModel.People = userManager.GetUserShowablePeopleList(currentUser);
            }


            editIncomingLettersViewModel.OrganizationJson = getWhileLoopData();

            editIncomingLettersViewModel.Cases = caseManager.Read(null);
            editIncomingLettersViewModel.Posts = postManager.Read(null);


            editIncomingLettersViewModel.LetterFiles = new List<FileViewModel>();
            foreach (ReceiveLetterFile itemFile in editIncomingLettersViewModel.ReceivedLetter.ReceiveLetterFiles)
            {
                FileViewModel fileViewModel = new FileViewModel
                {
                    Content = itemFile.File.Content.GetContentBase64(itemFile.File.ContentType),
                    Id = itemFile.Id
                };
                editIncomingLettersViewModel.LetterFiles.Add(fileViewModel);
            }
            editIncomingLettersViewModel.LetterAppendages = new List<FileViewModel>();
            foreach (ReceiveAppendage itemFile in editIncomingLettersViewModel.ReceivedLetter.Appendages)
            {
                FileViewModel fileViewModel = new FileViewModel
                {
                    Content = itemFile.File.Content.GetContentBase64(itemFile.File.ContentType),
                    Id = itemFile.Id
                };
                editIncomingLettersViewModel.LetterAppendages.Add(fileViewModel);
            }

            //Process Letter Parent
            //Set Parent Letter
            try
            {
                switch (editIncomingLettersViewModel.ReceivedLetter.ParentType)
                {
                    case ParentType.Receive:
                        var parent = receiveLetterManager.GetParent(editIncomingLettersViewModel.ReceivedLetter.Id);
                        editIncomingLettersViewModel.ReceivedLetter.ParentReceiveLetter = parent;

                        break;
                    case ParentType.Send:
                        var parent2 = receiveLetterManager.GetParentOut(editIncomingLettersViewModel.ReceivedLetter.Id);
                        editIncomingLettersViewModel.ReceivedLetter.ParentSendLetterOut = parent2;

                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
            //try
            //{
            //    var parent = receiveLetterManager.GetParent(editIncomingLettersViewModel.ReceivedLetter.Id);
            //    editIncomingLettersViewModel.ParentRowNumber = parent.RowNumber;
            //    editIncomingLettersViewModel.ReceivedLetter.ParentReceiveLetter = receiveLetterManager.Read(parent.Id);
            //}
            //catch
            //{
            //    editIncomingLettersViewModel.ParentRowNumber = "";
            //}

            return View(editIncomingLettersViewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditIncomingLettersViewModel editIncomingLettersViewModel)
        {
            ReceiveLetterManager receiveLetterManager = new ReceiveLetterManager();
            ReceivedLetter oldLetter = receiveLetterManager.Read(editIncomingLettersViewModel.ReceivedLetter.Id);

            #region temp

            TempReceiveLetterManager tempReceiveLetterManager = new TempReceiveLetterManager();
            //Get Letter before Edit
            ReceivedLetter receivedLetter = tempReceiveLetterManager.ReturnReceivedLetter(editIncomingLettersViewModel.ReceivedLetter.Id);
            if (receivedLetter == null)
            {
                throw new Exception("نامه مورد نظر یافت نگردید.");
            }
            TempReceivedLetter tempReceivedLetter = new TempReceivedLetter
            {
                User = receivedLetter.User,
                Sender = receivedLetter?.Sender ?? null,
                ParentType = receivedLetter.ParentType,
                AppendageContent = receivedLetter.AppendageContent,
                LetterFileContent = receivedLetter.LetterFileContent,
                ArchiveDateTime = receivedLetter.ArchiveDateTime,
                LetterDateTime = receivedLetter.LetterDateTime,
                ModifyDateTime = receivedLetter.LastModifyDateTime,
                Assortment = receivedLetter.Assortment,
                Cases = new List<Case>(),
                RowNumber = receivedLetter.RowNumber,
                Description = receivedLetter.Description,
                ReceivedLetter = receivedLetter,
                ParentSendLetterOutTemp = receivedLetter.ParentSendLetterOut,
                ParentReceiveLetter = receivedLetter.ParentReceiveLetter,
                TempReceiveLetterFiles = new List<TempReceiveLetterFile>(),
                TempReceiveAppendages = new List<TempReceiveAppendage>(),
                LetterFileAppendageContent = receivedLetter.LetterFileAppendageContent,
                Active = true
            };
            //Temp Collection of Cases
            foreach (var item in receivedLetter.Cases)
            {
                tempReceivedLetter.Cases.Add(item);
            }
            //Copy files to temp
            foreach (var item in receivedLetter.ReceiveLetterFiles)
            {
                TempReceiveLetterFile newTempReceiveLetterFile = new TempReceiveLetterFile { File = item.File };
                tempReceivedLetter.TempReceiveLetterFiles.Add(newTempReceiveLetterFile);
            }
            //Copy Appendages to temp
            foreach (var item in receivedLetter.Appendages)
            {
                TempReceiveAppendage newTempReceiveLetterFile = new TempReceiveAppendage { File = item.File };
                tempReceivedLetter.TempReceiveAppendages.Add(newTempReceiveLetterFile);
            }

            tempReceiveLetterManager.Add(tempReceivedLetter);
            tempReceiveLetterManager.saveChanges();

            #endregion

            oldLetter.Assortment = editIncomingLettersViewModel.ReceivedLetter.Assortment;

            //oldLetter.RowNumber = editIncomingLettersViewModel.ReceivedLetter.RowNumber;
            oldLetter.Description = editIncomingLettersViewModel.ReceivedLetter.Description;

            //oldLetter.ParentReceiveLetter = editIncomingLettersViewModel.ReceivedLetter.ParentReceiveLetter;

            if (!string.IsNullOrEmpty(editIncomingLettersViewModel.LetterDateTime))
            {
                oldLetter.LetterDateTime = editIncomingLettersViewModel.LetterDateTime.toEnglishNumber().ToGeorgianDateTime();
            }

            receiveLetterManager.UpdateReceiveLetterFiles(oldLetter, editIncomingLettersViewModel.FileUploadLetters, editIncomingLettersViewModel.FilesId);
            receiveLetterManager.UpdateReceiveLetterAppendages(oldLetter, editIncomingLettersViewModel.FileUploadAppendages, editIncomingLettersViewModel.AppendagesId);

            oldLetter.LastModifyDateTime = DateTime.Now;

            if (oldLetter.User.Id != new Guid(User.Identity.Name))
            {
                receiveLetterManager.AddUser(oldLetter, new Guid(User.Identity.Name));
            }

            //oldLetter.ParentReceiveLetter = editIncomingLettersViewModel.ParentReceiveletterId != null ? receiveLetterManager.Read((Guid)editIncomingLettersViewModel.ParentReceiveletterId) : null;


            if (editIncomingLettersViewModel.PersonGuid != Guid.Empty || editIncomingLettersViewModel.OrganizationGuid != Guid.Empty)
                receiveLetterManager.EditSender(oldLetter, editIncomingLettersViewModel.OrganizationGuid, editIncomingLettersViewModel.PersonGuid, editIncomingLettersViewModel.PostGuid);



            receiveLetterManager.EditCases(oldLetter, editIncomingLettersViewModel.CasesGuid);



            oldLetter.RowNumber = editIncomingLettersViewModel.RowNumber;
            oldLetter.ParentType = editIncomingLettersViewModel.ReceivedLetter.ParentType;
            //Set Parent Letter
            receiveLetterManager.SetParent(oldLetter, editIncomingLettersViewModel.ParentletterId);



            receiveLetterManager.GeneratePDF(oldLetter);
            receiveLetterManager.Update(oldLetter);

            receiveLetterManager.saveChanges();
            return RedirectToAction("Index");
        }

        public FileResult DownloadAppendage(Guid id)
        {
            ReceiveLetterAppendageManager receiveLetterAppendageManager = new ReceiveLetterAppendageManager();
            ArchiveFile file = receiveLetterAppendageManager.Read(id)?.File;
            if (file != null)
            {
                byte[] fileBytes = file.Content;
                string fileName = file.Name;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public FileResult DownloadFile(Guid id)
        {
            ReceiveLetterFileManager receiveLetterFileManager = new ReceiveLetterFileManager();
            ArchiveFile file = receiveLetterFileManager.Read(id)?.File;
            if (file != null)
            {
                byte[] fileBytes = file.Content;
                string fileName = file.Name;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public FileResult DownloadLetterContent(Guid id)
        {
            ReceiveLetterManager recieLetterManager = new ReceiveLetterManager();
            ReceivedLetter receivedLetter = new ReceivedLetter();
            receivedLetter = recieLetterManager.Read(id);
            byte[] file = receivedLetter?.LetterFileContent;
            if (file != null && file.Length > 0)
            {
                string fileName = id + ".pdf";
                return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
        }
    }
}