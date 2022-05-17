using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BTShowMechAffinity
{
    public static class Logger
    {
        internal static string LogFilePath => Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + "\\Log.txt";
        internal static string LogFileDir => Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + "\\";

        public static void Log(Exception ex)
        {
            using (StreamWriter streamWriter = new StreamWriter(Logger.LogFilePath, true))
            {
                streamWriter.WriteLine("Message: " + ex.Message);
                streamWriter.WriteLine("StackTrace: " + ex.StackTrace);
                streamWriter.WriteLine("Source: " + ex.Source);
                streamWriter.WriteLine(string.Format("Data: {0}", (object)ex.Data));
            }
        }

        public static void LogDebug(string line)
        {
            using (StreamWriter streamWriter = new StreamWriter(Logger.LogFilePath, true))
                streamWriter.WriteLine(line);
        }

        public static void Log(string line)
        {
            using (StreamWriter streamWriter = new StreamWriter(Logger.LogFilePath, true))
                streamWriter.WriteLine(line);
        }

        /// <summary>
        /// Writes an object as JSON text.
        /// </summary>
        /// <param name="data">the object to log</param>
        /// <param name="logFile">if not set, defaults to the default log.txt, otherwise
        /// the name of the file to write to the mod's directory.</param>
        /// <example>
        ///     LogJson(foo)
        ///     LogJson(foo, "foo.txt")
        /// </example>
        public static void LogJson(object data, string logFile = "")
        {

            logFile = String.IsNullOrEmpty(logFile) ? LogFilePath : Path.Combine(LogFileDir, logFile);

            using (StreamWriter streamWriter = new StreamWriter(logFile, true))
                streamWriter.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void Clear()
        {
            using (StreamWriter streamWriter = new StreamWriter(Logger.LogFilePath, false))
                streamWriter.WriteLine(DateTime.Now.ToLongTimeString() + "  Cleared");
        }
    }
}