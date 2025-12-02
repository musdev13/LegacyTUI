using System.Diagnostics;

namespace LegacyTUIComp.Instances
{
    public partial class Methods
    {
        public static void runMC(string instancePath, string instanceVersion, bool runInstant)
        {
            string command = "java";
            string args = $"-jar \"{Path.Combine(LegacyTUIComp.Methods.WorkspaceDir(), "bootstrap.jar")}\" {((runInstant) ? "--launch" : "")} --directory \"{instancePath}\" --version \"{instanceVersion}\"";
            Console.WriteLine($"{command} {args}");

            var psi = new ProcessStartInfo()
            {
                FileName = command,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = psi;

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null)
                    Console.WriteLine(e.Data);
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null)
                    Console.WriteLine("Error: " + e.Data);
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
        }
    }
}