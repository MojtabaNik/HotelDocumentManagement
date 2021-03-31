using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Archive.ViewModel;
using BusinessModel;
using EntityModel;

namespace Archive.Controllers
{
    public class CaseController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            CaseManager caseManager = new CaseManager();
            CaseViewModel caseViewModel = new CaseViewModel
            {
                Case = caseManager.Read(null),
                PageId = "Cases"
            };
            return View(caseViewModel);
        }

        public string getWhileLoopData()
        {
            CaseManager caseManager = new CaseManager();

            var htmlStr = new System.Text.StringBuilder();
            List<Case> empty = new List<Case>();
            empty = caseManager.GetChildItems(caseManager.Read(null).First().Id, empty);
            htmlStr.Append("<tr><td>");
            htmlStr.Append(caseManager.Read(null).First().Id);
            htmlStr.Append("</td><td>");
            htmlStr.Append("");
            htmlStr.Append("</td><td>");
            htmlStr.Append(caseManager.Read(null).First().Name);
            htmlStr.Append("</td><td>");
            htmlStr.Append("خروجی: " + "0");
            htmlStr.Append("</td><td>");
            htmlStr.Append("ورودی: " + "0");
            htmlStr.Append("</td></tr>");
            foreach (var item in empty)
            {
                string parent = "";
                if (item.ParentCase != null)
                {
                    parent = item.ParentCase.Id.ToString();
                }
                Guid id = item.Id;
                string name = item.Name;
                htmlStr.Append("<tr><td>");
                htmlStr.Append(id);
                htmlStr.Append("</td><td>");
                htmlStr.Append(parent);
                htmlStr.Append("</td><td>");
                htmlStr.Append(name);
                htmlStr.Append("</td><td>");
                htmlStr.Append("خروجی: " + "0");
                htmlStr.Append("</td><td>");
                htmlStr.Append("ورودی: " + "0");
                htmlStr.Append("</td></tr>");

            }
            return htmlStr.ToString();
        }
        [HttpPost]
        public ActionResult Add(CaseViewModel caseViewModel)
        {
            if (ModelState.IsValid)
            {
                CaseManager caseManager = new CaseManager();
                Case ParentCase = new Case();
                Case newCase = new Case();
                ParentCase = caseManager.Read(x => x.Id == caseViewModel.ParentCaseGUID).First();
                newCase.Name = caseViewModel.Name;
                newCase.ParentCase = ParentCase;
                caseManager.Add(newCase);
                caseManager.saveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id)
        {
            CaseManager caseManager = new CaseManager();
            CaseViewModel caseViewModel = new CaseViewModel { Case = caseManager.Read(null) };
            caseViewModel.PageId = "Cases";
            caseViewModel.SelectedCase = caseManager.Read(id);
            caseViewModel.SelectedCaseParent = caseManager.GetParent(id);
            return View(caseViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EditCaseViewModel editCaseViewModel)
        {
            CaseManager caseManager = new CaseManager();
            var oldCase = caseManager.Read(editCaseViewModel.CaseID);
            oldCase.Name = editCaseViewModel.Name;
            oldCase.ParentCase =
                caseManager.Read(editCaseViewModel.ParentCaseGUID);
            caseManager.Update(oldCase);

          caseManager.saveChanges();
            return RedirectToAction("Index");
        }
    }
}