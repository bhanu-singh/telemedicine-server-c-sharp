using ImTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImTech.Service.ViewModels
{
    public class ViewModelBase
    {
        public ViewModelBase()
        {
            CreatedOn = new InternalDateTime();
            ModifiedOn = new InternalDateTime();
        }
        public int TanentId { get; set; }
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public InternalDateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public InternalDateTime ModifiedOn { get; set; }

        public int CreatedByEntity { get; set; }

        public int ModifiedEntity { get; set; }

        public bool IsDeleted { get; set; }

        public long LongId { get; set; }
    }
}