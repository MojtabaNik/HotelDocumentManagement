using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Archive.ViewModel;
using BusinessModel;
using DBProvider;
using EntityModel;
namespace Archive.Controllers
{
    public class PersonController : Controller
    {
        public ActionResult Index()
        {
            PersonManager personManager = new PersonManager();
            PersonViewModel personViewModel = new PersonViewModel
            {
                People = personManager.Read(null),
                PageId = "ShowPeople"
            };
            personManager.Dispose();
            return View(personViewModel);
        }
        [HttpGet]
        public ActionResult Add()
        {
            AddPersonViewModel addPersonViewModel = new AddPersonViewModel { PageId = "AddPerson" };
            PostManager postManager = new PostManager();
            UserManager userManager = new UserManager();
            User currentUser = userManager.Read(new Guid(User.Identity.Name));

            addPersonViewModel.Organizations = userManager.GetUserShowableOrganizations(currentUser);
            addPersonViewModel.Posts = postManager.Read(null);
            return View(addPersonViewModel);
        }
        [HttpPost]
        public ActionResult Add(AddPersonViewModel addPersonViewModel)
        {
            addPersonViewModel.Person.Avatar = new ArchiveFile();


            if (addPersonViewModel.fileUpload != null && addPersonViewModel.fileUpload.ContentLength>0&&addPersonViewModel.fileUpload.ContentType.Contains("image"))
            {
                //Get Content of image
                MemoryStream target = new MemoryStream();
                addPersonViewModel.fileUpload.InputStream.CopyTo(target);
                addPersonViewModel.Person.Avatar.Content = target.ToArray();
                addPersonViewModel.Person.Avatar.Name = addPersonViewModel.fileUpload.FileName;
                addPersonViewModel.Person.Avatar.ContentType = addPersonViewModel.fileUpload.ContentType;
            }
           

            if (!string.IsNullOrEmpty(addPersonViewModel.BirthDay))
            {
                addPersonViewModel.Person.BirthDay = addPersonViewModel.BirthDay.toEnglishNumber().ToGeorgianDateTime();
            }




            PersonManager personManager = new PersonManager();
            //Add Organizations
            personManager.AddPostOrganization(addPersonViewModel.Person, addPersonViewModel.SelectedOrganizations,addPersonViewModel.SelectedPosts);
            personManager.Add(addPersonViewModel.Person);
            personManager.saveChanges();

            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            if (id != null && id!=Guid.Empty)
            {
                PersonManager personManager = new PersonManager();
                UserManager userManager = new UserManager();
                PostManager postManager = new PostManager();
                EditPersonViewModel editPersonViewModel = new EditPersonViewModel
                {
                    PageId = "ShowPeople",
                    Person = personManager.Read((Guid)id)
                };
                if (editPersonViewModel.Person == null)
                    return RedirectToAction("Index");
                editPersonViewModel.Organizations = userManager.GetUserShowableOrganizations(userManager.Read(new Guid(User.Identity.Name)));
                editPersonViewModel.Posts = postManager.Read(null);

                return View(editPersonViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPersonViewModel editPersonViewModel)
        {
            PersonManager personManager = new PersonManager();
            //List<Guid> GuidList = new List<Guid>();
            //List<Guid> PostsGuidList = new List<Guid>();
            if (editPersonViewModel.Person.Avatar == null)
            {
                editPersonViewModel.Person.Avatar = new ArchiveFile();
            }
            if (editPersonViewModel.fileUpload != null && editPersonViewModel.fileUpload.ContentLength > 0 && editPersonViewModel.fileUpload.ContentType.Contains("image"))
            {
                //Get Content of image

                MemoryStream target = new MemoryStream();
                editPersonViewModel.fileUpload.InputStream.CopyTo(target);
                editPersonViewModel.Person.Avatar.Content = target.ToArray();
                editPersonViewModel.Person.Avatar.Name = editPersonViewModel.fileUpload.FileName;
                editPersonViewModel.Person.Avatar.ContentType = editPersonViewModel.fileUpload.ContentType;
            }
            else if (!editPersonViewModel.Flag)
            {
                PersonManager otherManager = new PersonManager();
                Person oldPerson = otherManager.Read(editPersonViewModel.Person.Id);
                otherManager.Dispose();
                editPersonViewModel.Person.Avatar.Content = oldPerson.Avatar.Content;
                editPersonViewModel.Person.Avatar.Name = oldPerson.Avatar.Name;
                editPersonViewModel.Person.Avatar.ContentType = oldPerson.Avatar.ContentType;
            }
           

            if (!string.IsNullOrEmpty(editPersonViewModel.BirthDay))
            {
                editPersonViewModel.Person.BirthDay = editPersonViewModel.BirthDay.toEnglishNumber().ToGeorgianDateTime();
            }

            personManager.Update(editPersonViewModel.Person);
            personManager.EditPostOrganization(editPersonViewModel.Person.Id, editPersonViewModel.SelectedOrganizations,editPersonViewModel.SelectedPosts);
            personManager.saveChanges();


            return RedirectToAction("Index");
        }
    }
}
