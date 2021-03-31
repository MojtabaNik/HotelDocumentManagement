using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;

namespace Archive.ViewModel
{
    public class AdvanceSearchViewModel:BaseViewModel
    {
        public List<SendLetter> SendLetters { get; set; }
        public List<ReceivedLetter> ReceivedLetters { get; set; }

    }
}