using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class PrescreptionViewModel : ViewModelBase
    {
        public string PrescreptionText { get; set; }

        public int ConsultationId { get; set; }

        public int Type { get; set; }
    }
}