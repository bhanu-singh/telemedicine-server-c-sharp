using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class CaseNoteViewModel:ViewModelBase
    {
        public int ConsultationId { get; set; }

        public string NoteText { get; set; }

        public int Type { get; set; }
    }
}