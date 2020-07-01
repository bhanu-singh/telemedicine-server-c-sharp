using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.Model
{
    public class BaseModel
    {
        public BaseModel()
        {
            Success = true;
            ErrorMessage = string.Empty;
            CreatedOn = new InternalDateTime();
            ModifiedOn = new InternalDateTime();
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public int TenantID { get; set; }
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public InternalDateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public InternalDateTime ModifiedOn { get; set; }

        public int CreatedByEntity { get; set; }

        public int ModifiedByEntity { get; set; }

        public bool IsDeleted { get; set; }

        public Int16 From { get; set; }

        public long LongId { get; set; }

    }
}
