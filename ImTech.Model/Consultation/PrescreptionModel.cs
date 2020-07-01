using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
   public class PrescreptionModel:BaseModel
    {
        public string PrescreptionText { get; set; }

        public int ConsultationId { get; set; }

        public int Type { get; set; }
    }
}
