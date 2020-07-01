using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
   public class CaseNoteModel:BaseModel
    {
        public int ConsultationId { get; set; }

        public string NoteText { get; set; }

        public int Type { get; set; }
    }
}
