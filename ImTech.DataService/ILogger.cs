using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Model;
namespace ImTech.DataServices
{
    public interface ILogger
    {
        void LogInfo(LogMessage logMessage);

        void LogWarning(LogMessage logMessage);

        void LogError(LogMessage logMessage);

        void LogEmail(LogMessage logMessage);
    }
}
