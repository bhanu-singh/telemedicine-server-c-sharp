using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class DegreeModel : BaseModel
    {
        public string DegreeName { get; set; }
    }

    public class DeseaseModel : BaseModel
    {
        public string DeseaseName { get; set; }
    }

    public class SpecializationModel : BaseModel
    {
       
        public string SpecializationName { get; set; }

        public int ParentID { get; set; }
    }
}
