using System;
using System.IO;

namespace LegacyTUIComp
{
    public partial class Methods
    {
        private static string? _workspaceDir = null;

        public static string WorkspaceDir()
        {
            if (_workspaceDir != null)
                return _workspaceDir;

            string currentDir = Directory.GetCurrentDirectory();
            string exeDir = AppContext.BaseDirectory;

            if (Directory.GetFiles(currentDir, "*.csproj").Length > 0)
            {
                _workspaceDir = Path.Combine(currentDir, "internal");
                Directory.CreateDirectory(_workspaceDir);
            }
            else
            {
                _workspaceDir = exeDir;
            }

            return _workspaceDir;
        }
    }
}
