using System.Collections.Generic;
using Archive.ViewModel.Templates;
using EntityModel;

namespace Archive.ViewModel
{
    public class TemplateViewModel:BaseViewModel
    {
        public List<TemplateVM> Templates { get; set; }
    }
}