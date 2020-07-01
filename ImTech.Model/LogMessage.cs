using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class LogMessage
    {
        public LogMessage()
        {
            CreationDate = new InternalDateTime();
        }
        public int Id { get; set; }

        public string Application { get; set; }
        public string Module { get; set; }

        public string Summary { get; set; }

        public string Exception { get; set; }
        public string IP { get; set; }

        public int UserType { get; set; }
        public int? UserId { get; set; }

        public InternalDateTime CreationDate { get; set; }
    }
}
