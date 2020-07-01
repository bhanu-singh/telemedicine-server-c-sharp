using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Model;
namespace ImTech.DataServices
{
    public abstract class FileSystem : IFileSystem
    {
        private string _uniqueDateFormat;

        public FileSystem()
        {
            _uniqueDateFormat = "MMddyyyyHHmmsstt";
        }
        protected virtual string UniqueFileNameFormat
        {
            get
            {
                return _uniqueDateFormat;
            }

        }
        public virtual string GetGeneratedFileName(NonSecureFileModel fileModel, int userId)
        {
            string fileName = fileModel.ActualFileName + fileModel.FileDocType + Guid.NewGuid() + fileModel.FileExtension;
            return fileName;
        }


        public abstract bool Write(NonSecureFileModel fileModel);

        public abstract NonSecureFileModel Read(NonSecureFileModel fileModel);

        public abstract bool Delete(NonSecureFileModel fileModel);

        public abstract string GetFolderPath(NonSecureFileModel filemodel);

        //public abstract string GetGeneratedFileName(FileModel fileModel, int userId);

        internal abstract string CreateAbsoultePath(string docType);
    }

}
