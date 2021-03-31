using BusinessModel;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Archive.ViewModel
{
    public class OutgoingLettersViewModel:BaseViewModel
    {

        public List<SendLetter> ListSendLetters { get; set; }

    }
}