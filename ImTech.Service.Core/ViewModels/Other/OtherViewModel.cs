using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels.Other
{
    public class DegreeViewModel : ViewModelBase
    {
        public string DegreeName { get; set; }
    }

    public class SpecializationViewModel : ViewModelBase
    {
        public string SpecializationName { get; set; }
    }

    public class DeseasesViewModel : ViewModelBase
    {
        public string DeseaseName { get; set; }
    }


}