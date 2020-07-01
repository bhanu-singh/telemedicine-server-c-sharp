using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class NonSecureFileModel : BaseModel
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string FileDocType { get; set; }

        public string FileFullPath { get; set; }

        public string ActualFileName { get; set; }

        public string EntityType { get; set; }

        public byte[] Contents { get; set; }

        public Stream ContentStream { get; set; }

    }
}
