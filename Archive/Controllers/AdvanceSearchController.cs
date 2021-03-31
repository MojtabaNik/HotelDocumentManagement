using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Archive.ViewModel;
using BusinessModel;
using EntityModel;

namespace Archive.Controllers
{
    public class AdvanceSearchController : Controller
    {
        //private DatabaseContext context;
        // GET: AdvanceSearch
        public ActionResult Index(Guid? letterID, ParentType? letterType)
        {
            if (letterID != null && letterID != Guid.Empty)
            {
                SendLetterManager sendLetterManager = new SendLetterManager();
                ReceiveLetterManager receiveLetterManager = new ReceiveLetterManager();
                Tuple<List<ReceivedLetter>, List<SendLetter>> myTuple;
                switch (letterType)
                {
                    case ParentType.Receive:
                        //var sendLetterChildrens = sendLetterManager.GetChildItems((Guid) letterID,
                        //    new List<SendLetter>());
                        myTuple = receiveLetterManager.GetChildItems((Guid)letterID, new Tuple<List<ReceivedLetter>, List<SendLetter>>(new List<ReceivedLetter>(), new List<SendLetter>()));
                        break;
                    case ParentType.Send:
                        myTuple = sendLetterManager.GetChildItems((Guid)letterID, new Tuple<List<ReceivedLetter>, List<SendLetter>>(new List<ReceivedLetter>(), new List<SendLetter>()));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(letterType), letterType, null);
                }

                //Initialize letters who is not child of this item
                AdvanceSearchViewModel advanceSearchViewModel = new AdvanceSearchViewModel
                {
                    ReceivedLetters = new List<ReceivedLetter>(),
                    SendLetters = new List<SendLetter>()
                };
                foreach (var item in receiveLetterManager.Read("1==1", "ParentReceiveLetter,ParentSendLetterOut").ToList())
                {
                    if (myTuple.Item1.All(x => x.Id != item.Id))
                        advanceSearchViewModel.ReceivedLetters.Add(item);
                }
                foreach (var item in sendLetterManager.Read("1==1", "ParentSendLetter,ParentReceivedLetterOut").ToList())
                {
                    if (myTuple.Item2.All(x => x.Id != item.Id))
                        advanceSearchViewModel.SendLetters.Add(item);
                }

                //AdvanceSearchViewModel advanceSearchViewModel = new AdvanceSearchViewModel
                //{
                //    SendLetters = sendLetterManager.Read(x => myTuple.Item2.All(y => y.Id != x.Id),null, "ParentSendLetterOut,ParentSendLetter").ToList(),
                //    ReceivedLetters = receiveLetterManager.Read(x => myTuple.Item1.All(y => y.Id != x.Id),null, "ParentReceivedLetterOut,ParentReceiveLetter").ToList()
                //};

                return View(advanceSearchViewModel);

            }
            else
            {
                SendLetterManager sendLetterManager = new SendLetterManager();
                ReceiveLetterManager receiveLetterManager = new ReceiveLetterManager();
                AdvanceSearchViewModel advanceSearchViewModel = new AdvanceSearchViewModel
                {
                    PageId = "IncomingAndOutgoings",
                    ReceivedLetters =
                        receiveLetterManager.Read("1==1", "ParentSendLetterOut,ParentReceiveLetter").ToList(),
                    SendLetters =
                        sendLetterManager.Read("1==1", "ParentSendLetter,ParentReceivedLetterOut").ToList()
                };
                return View(advanceSearchViewModel);
            }
            //}
        }
    }
}