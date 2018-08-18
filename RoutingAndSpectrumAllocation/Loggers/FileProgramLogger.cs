using System.IO;
using System.Threading.Tasks;

namespace RoutingAndSpectrumAllocation.Loggers
{
    public class FileProgramLogger : IProgramLogger
    {
        string Path { get; set; }

        public FileProgramLogger(string path)
        {
            Path = path;

            if (Directory.Exists(Path) == false)
                Directory.CreateDirectory(Path);

            File.WriteAllText(Path + $"\\log.txt", "");
        }

        public async Task LogInformation(string payload)
        {
            await File.AppendAllTextAsync(Path + $"\\log.txt", payload + "\n");
        }
    }
}
