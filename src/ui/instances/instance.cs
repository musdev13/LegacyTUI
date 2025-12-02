using System.Diagnostics;

namespace LegacyTUIComp.Instances
{
    public partial class UI
    {
        public static void Instance(string instanceName)
        {
            string instancePath = Path.Combine(Path.Combine(LegacyTUIComp.Methods.WorkspaceDir(),"instances"),instanceName);
            string? instanceVersion = LegacyTUIComp.Methods.GetFromFile(Path.Combine(instancePath,"LegacyTUI_data"),0);

            Console.Clear();
            Console.WriteLine($"{instanceName}\nversion: {instanceVersion}");
            
            string command = "java";
            string args = $"-jar \"{Path.Combine(LegacyTUIComp.Methods.WorkspaceDir(), "bootstrap.jar")}\" --launch --directory \"{instancePath}\" --version \"{instanceVersion}\"";
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
