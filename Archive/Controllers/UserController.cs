using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBProvider;
using BusinessModel;
using Archive.ViewModel;
using EntityModel;
using System.IO;
using System.Web.Security;
using Archive.Filters;

namespace Archive.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [AdminFilter]
        public ActionResult Index()
        {
            UserManager userManager = new UserManager();
            ShowAllUsersViewModel showAllUsersViewModel = new ShowAllUsersViewModel
            {
                Users = userManager.Read(null),
                PageId = "ShowUsers"
            };
            //userManager.Dispose();
            return View(showAllUsersViewModel);
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (Sessions.CurrentUser != null)
                return RedirectToAction("Index", "Dashboard");


            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                Sessions.ReturnUrl = returnUrl;
            }

            UserManager userManager = new UserManager();

       

            //Sessions.CurrentUser = userManager.Read(null, "FilterOrganizations,FilterPeople").FirstOrDefault();

            //SendLetterManager receiveLetterManager = new SendLetterManager();
            //MemoryStream stream = new MemoryStream(receiveLetterManager.Read(null).Last().LetterFileContent);
            //Aspose.Pdf.Document letterPdf = new Aspose.Pdf.Document(stream);

            ////letterPdf.Save(stream);
            ////Debug
            //letterPdf.Save(Server.MapPath("~/AppendagesOnly.pdf"));

            //this is for debug purposes only
            ShowAllUsersViewModel showAllUsersViewModel = new ShowAllUsersViewModel { Users = userManager.Read(null) };
            userManager.Dispose();
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([Bind(Include = "Username,password")]User user)
        {


            //return RedirectToAction("Index", "Dashboard");
            UserManager userManager = new UserManager();
            try
            {
                if (userManager.Authentication(user))
                {
                    //return Content(user.UserName + " " + user.password);
                    User currentUser =
                        userManager.Read(x => x.UserName == user.UserName, null, "FilterOrganizations,FilterPeople")
                            .FirstOrDefault();
                    Sessions.IsAdmin = currentUser.IsAdmin;
                    FormsAuthentication.SetAuthCookie(currentUser.Id.ToString(), false);
                    Sessions.CurrentUser = currentUser;
                    //Sessions.CurrentUser =
                    //    userManager.Read(x => x.UserName == user.UserName, null, "FilterOrganizations,FilterPeople")
                    //        .FirstOrDefault();
                    if (!string.IsNullOrEmpty(Sessions.ReturnUrl))
                    {
                        string go = Sessions.ReturnUrl;
                        Sessions.ReturnUrl = null;
                        return Redirect("~" + go);
                    }
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("error", "نام کاربری یا کلمه عبور اشتباه است.");
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return View("Login");
            }
            return null;
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Sessions.CurrentUser = null;
            Sessions.IsAdmin = false;
            return RedirectToAction("Login");
        }


        [AdminFilter]
        [HttpGet]
        public ActionResult Add()
        {
            AddUserViewModel addUserViewModel = new AddUserViewModel { PageId = "AddUser" };
            return View(addUserViewModel);
        }
        [AdminFilter]
        [HttpPost]
        public ActionResult Add(AddUserViewModel addUserViewModel)
        {
            addUserViewModel.PageId = "AddUser";
            //if (ModelState.IsValid)
            //{
                addUserViewModel.user.Avatar = new ArchiveFile();

                if (!string.IsNullOrEmpty(addUserViewModel.BirthDay))
                {
                    addUserViewModel.user.BirthDay = addUserViewModel.BirthDay.toEnglishNumber().ToGeorgianDateTime();
                }

                if (addUserViewModel.fileUpload != null && addUserViewModel.fileUpload.ContentLength > 0)
                {
                    //Get Content of image
                    MemoryStream target = new MemoryStream();
                    addUserViewModel.fileUpload.InputStream.CopyTo(target);
                    addUserViewModel.user.Avatar.Content = target.ToArray();
                    addUserViewModel.user.Avatar.Name = addUserViewModel.fileUpload.FileName;
                    addUserViewModel.user.Avatar.ContentType = addUserViewModel.fileUpload.ContentType;
                }

                if (addUserViewModel.repeatPassword == addUserViewModel.user.password)
                {
                    addUserViewModel.user.password = addUserViewModel.user.password.ToPassword();
                    UserManager um = new UserManager();
                    um.Add(addUserViewModel.user);
                    um.saveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("error", "کلمه عبور و تکرار آن باید یکسان باشند.");
                    return View(addUserViewModel);
                }


            //}
            //else
            //{
            //    return View(addUserViewModel);
            //}

        }

        [AdminFilter]
        [HttpGet]
        public ActionResult Edit(Guid? ID)
        {
            if (ID != null)
            {
                UserManager UM = new UserManager();
                EditUserViewModel editUserViewModel = new EditUserViewModel
                {
                    PageId = "ShowUsers",
                    user = UM.Read((Guid)ID)
                };
                if (editUserViewModel.user == null)
                    return RedirectToAction("Index");
                return View(editUserViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [AdminFilter]
        [HttpPost]
        ///[ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel editUserViewModel)
        {
            UserManager userManager = new UserManager();

            if (editUserViewModel.user.Avatar == null)
            {
                editUserViewModel.user.Avatar = new ArchiveFile();
            }
            if (editUserViewModel.fileUpload != null && editUserViewModel.fileUpload.ContentLength > 0)
            {
                //Get Content of image

                MemoryStream target = new MemoryStream();
                editUserViewModel.fileUpload.InputStream.CopyTo(target);
                editUserViewModel.user.Avatar.Content = target.ToArray();
                editUserViewModel.user.Avatar.Name = editUserViewModel.fileUpload.FileName;
                editUserViewModel.user.Avatar.ContentType = editUserViewModel.fileUpload.ContentType;
            }
            else if (!editUserViewModel.Flag)
            {
                UserManager otherManager = new UserManager();
                User oldUser = otherManager.Read(editUserViewModel.user.Id);
                otherManager.Dispose();
                editUserViewModel.user.Avatar.Content = oldUser.Avatar.Content;
                editUserViewModel.user.Avatar.Name = oldUser.Avatar.Name;
                editUserViewModel.user.Avatar.ContentType = oldUser.Avatar.ContentType;
            }
            if (!String.IsNullOrEmpty(editUserViewModel.user.password) &&
                editUserViewModel.user.password == editUserViewModel.repeatPassword)
            {

                editUserViewModel.user.password = editUserViewModel.user.password.ToPassword();
            }
            else
            {
                UserManager otherManager = new UserManager();
                User oldUser = otherManager.Read(editUserViewModel.user.Id);
                editUserViewModel.user.password = oldUser.password;
            }
            //BirthDay Bug fix
            if (!string.IsNullOrEmpty(editUserViewModel.BirthDay))
            {
                editUserViewModel.user.BirthDay = editUserViewModel.BirthDay.toEnglishNumber().ToGeorgianDateTime();
            }

            userManager.Update(editUserViewModel.user);
            userManager.saveChanges();

            return RedirectToAction("Index");
        }
    }
}