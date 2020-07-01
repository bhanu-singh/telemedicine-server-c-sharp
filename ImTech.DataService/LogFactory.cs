using ImTech.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImTech.DataServices
{
    public class LogFactory
    {
        public static ILogger Create()
        {
            IDataBaseService dataBaseService = new DataBaseService();
            return new Logger(dataBaseService);
        }
    }
}
