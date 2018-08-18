using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.FileLogger
{
    public class JsonFileLogger : IFileLogger
    {
        string FileLoggingPath { get; set; }

        public JsonFileLogger(string fileLoggingPath)
        {
            this.FileLoggingPath = fileLoggingPath;
        }

        public async Task WriteLog(string Id, object obj, string subFolder = "")
        {
            string path = FileLoggingPath;

            if (string.IsNullOrEmpty(subFolder) == false)
                path += "\\" + subFolder.Trim('\\');

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            await File.WriteAllTextAsync(path + $"\\{Id}.json", JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
