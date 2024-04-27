using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NewMasApp.ExternalComponents
{
    public class Logger
    {
        private static readonly object lockObject = new object();
        private static Logger instance;
        private string logFilePath;

        private Logger()
        {
            BuildLogsPath();
        }

        private void BuildLogsPath()
        {
            string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            logFilePath = Path.Combine(assemblyDirectory, "log.txt");
        }

        /// <summary>
        /// getInstance - return the instance of the logger singelton
        /// </summary>
        /// <returns></returns>
        public static Logger getInstance()
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        /// <summary>
        /// Log - Writes logs into the the log file
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            lock (lockObject)
            {
                try
                {
                    string logDirectory = Path.GetDirectoryName(logFilePath);
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine();
                        writer.WriteLine($"{DateTime.Now}: {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }
    }
}
