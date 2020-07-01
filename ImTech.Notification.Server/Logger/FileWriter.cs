using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImTech.Notification.Server
{
    public class FileWriter
    {

        public static void LogFileWrite(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string logFilePath = ConfigurationManager.LogFolderPath;

                logFilePath = logFilePath + "NotificationLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

                if (logFilePath.Equals("")) return;
                #region Create the Log file directory if it does not exists 

                DirectoryInfo logDirInfo = null;
                FileInfo logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();

                #endregion Create the Log file directory if it does not exists

                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(DateTime.Now + ":" + message);
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
            }

        }
    }
}

