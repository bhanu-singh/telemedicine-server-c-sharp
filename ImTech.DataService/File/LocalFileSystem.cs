using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Model;
using System.IO;

namespace ImTech.DataServices
{
    public class LocalFileSystem : FileSystem
    {
        public override bool Write(NonSecureFileModel fileModel)
        {
            using (var fileStream = System.IO.File.Create(fileModel.FileFullPath + fileModel.FileName))
            {
                //fileModel.ContentStream.ReadTimeout = 4000;
                fileModel.ContentStream.Position = 0;
                fileModel.ContentStream.Seek(0, SeekOrigin.Begin);
                fileModel.ContentStream.CopyTo(fileStream);
            }
            fileModel.ContentStream.Dispose();
            fileModel.ContentStream = null;
            return true;
        }

        public override NonSecureFileModel Read(NonSecureFileModel fileModel)
        {
            if (!System.IO.File.Exists(fileModel.FileFullPath + fileModel.FileName))
            {
                throw new FileNotFoundException("File not exist.");
            }

            fileModel.Contents = System.IO.File.ReadAllBytes(fileModel.FileFullPath + fileModel.FileName);

            return fileModel;
        }

        public override bool Delete(NonSecureFileModel fileModel)
        {
            if (!System.IO.File.Exists(fileModel.FileFullPath + fileModel.FileName))
            {
                return false;
            }

            System.IO.File.Delete(fileModel.FileFullPath + fileModel.FileName);

            return true;
        }

        public override string GetFolderPath(NonSecureFileModel filemodel)
        {
            string fileFullPath = CreateAbsoultePath(filemodel.FileDocType);
            if (!Directory.Exists(fileFullPath))
                Directory.CreateDirectory(fileFullPath);
            return fileFullPath;
        }
        internal override string CreateAbsoultePath(string docType)
        {
            return ConfigurationManager.FileUploadPath + ConfigurationManager.FileSaparator + docType + ConfigurationManager.FileSaparator;
        }
    }
}
