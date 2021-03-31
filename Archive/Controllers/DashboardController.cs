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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            SendLetterManager sendLetterManager = new SendLetterManager();
            ReceiveLetterManager ReceiveLetterManager = new ReceiveLetterManager();
            UserManager userManager = new UserManager();
            ActivityLogManager activityLogManager = new ActivityLogManager();
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.PageId = "Dashboard";
            dashboardViewModel.CountOfReceiveLetter = ReceiveLetterManager.Count;
            dashboardViewModel.CountOfSendLetter = sendLetterManager.Count;
            dashboardViewModel.CountOfUser = userManager.Count;
            //dashboardViewModel.activityLog = new List<ActivityLog>();
            //dashboardViewModel.activityLog = activityLogManager.Read(x=>x.TimeStamp >= System.Data.Entity.DbFunctions.AddDays(new DateTime(),-1),null,0,false);
            return View(dashboardViewModel);
        }
    }
}