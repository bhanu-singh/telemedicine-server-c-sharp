using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Model;
namespace ImTech.DataServices
{
    public interface IFileSystem
    {
        bool Write(NonSecureFileModel fileModel);

        NonSecureFileModel Read(NonSecureFileModel fileModel);

        bool Delete(NonSecureFileModel fileModel);
    }

}
