using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataServices.File
{
    public class FileServiceFactory
    {

        public static FileSystem GetFileSystem()
        {
            FileSystem fileSystem = null;
            if (ConfigurationManager.FileSystem == "local")
            {
                fileSystem = new LocalFileSystem();
            }
            else if (ConfigurationManager.FileSystem == "aws")
            {
                fileSystem = new AWSFileSystem();
            }

            return fileSystem;
        }
    }
}
