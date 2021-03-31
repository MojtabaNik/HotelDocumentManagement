using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Archive.ViewModel;
using BusinessModel;
using EntityModel;

namespace Archive.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index(Guid? id)
        {
            PostViewModel postViewModel = new PostViewModel();
            PostManager postManager = new PostManager();
            postViewModel.PageId = "Post";
            postViewModel.posts = postManager.Read(null);
            postViewModel.isEdit = !string.IsNullOrEmpty(id.ToString());
            if (postViewModel.isEdit)
                postViewModel.post = postManager.Read((Guid) id);
            return View(postViewModel);
        }

        [HttpPost]
        public ActionResult Add(PostViewModel postViewModel)
        {
            PostManager postManager = new PostManager();
            postManager.Add(postViewModel.post);
            postManager.saveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(PostViewModel postViewModel)
        {
            PostManager postManager = new PostManager();
            postManager.Update(postViewModel.post);
            postManager.saveChanges();
            return RedirectToAction("Index");
        }
    }
}