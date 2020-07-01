using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model.File
{
    public class UserFileMappingModel : BaseModel
    {
        public SecureFileModel SecureModel { get; set; }

        public NonSecureFileModel NonSecureModel { get; set; }
    }
}
