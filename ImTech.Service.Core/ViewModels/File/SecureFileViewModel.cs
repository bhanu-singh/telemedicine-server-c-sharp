using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class SecureFileViewModel : ViewModelBase
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string FileDocType { get; set; }

        public string FileFullPath { get; set; }

        public string ActualFileName { get; set; }

        public string EntityType { get; set; }
    }
}