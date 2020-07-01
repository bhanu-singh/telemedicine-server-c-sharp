using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataBase
{
    public interface IDataFilter
    {
        Parameter[] GetFilters();
    }
}
