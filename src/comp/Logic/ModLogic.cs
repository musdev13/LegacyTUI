using LegacyTUIComp;
using LegacyTUI.resManager.mods.global;

namespace LegacyTUIComp.Logic
{
    public class ModLogic
    {
        public static List<string> ListMods(string instanceName)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string instancePath = Path.Combine(workspaceDir, "instances", instanceName);
            string modsPath = Path.Combine(instancePath, "mods");

            if (!Directory.Exists(modsPath))
            {
                return new List<string>();
            }

            return Directory.GetFiles(modsPath)
                            .Select(Path.GetFileName)
                            .ToList()!;
        }

        public static void InstallMod(string instanceName, string slug)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string instancePath = Path.Combine(workspaceDir, "instances", instanceName);
            string dataFile = Path.Combine(instancePath, "LegacyTUI_data");

            if (!File.Exists(dataFile))
            {
                throw new Exception($"Instance '{instanceName}' data not found.");
            }

            string[] data = File.ReadAllLines(dataFile);
            if (data.Length < 2)
            {
                throw new Exception($"Instance '{instanceName}' data is corrupted.");
            }
            string version = data[0];
            string tag = data[1]; // loader

            string modsFolder = Path.Combine(instancePath, "mods");
            Directory.CreateDirectory(modsFolder);

            LegacyTUI.resManager.mods.global.Methods.installMod(slug, modsFolder, version, tag);
        }

        public static void DeleteMod(string instanceName, string fileName)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string modsPath = Path.Combine(workspaceDir, "instances", instanceName, "mods");
            string filePath = Path.Combine(modsPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new Exception($"Mod '{fileName}' not found in instance '{instanceName}'.");
            }
        }

        public static void DisableMod(string instanceName, string fileName)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string modsPath = Path.Combine(workspaceDir, "instances", instanceName, "mods");
            string filePath = Path.Combine(modsPath, fileName);
            string disabledPath = filePath + ".disabled";

            if (File.Exists(filePath))
            {
                if (File.Exists(disabledPath)) File.Delete(disabledPath);
                File.Move(filePath, disabledPath);
            }
            else
            {
                throw new Exception($"Mod '{fileName}' not found.");
            }
        }

        public static void EnableMod(string instanceName, string fileName)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string modsPath = Path.Combine(workspaceDir, "instances", instanceName, "mods");
            // User puts "mod.jar.disabled" or just "mod.jar" and we expect .disabled?
            // Prompt says: -e "mod.jar.disabled"
            string filePath = Path.Combine(modsPath, fileName);
            string enabledPath = filePath.EndsWith(".disabled") ? filePath.Substring(0, filePath.Length - 9) : filePath;

            if (!filePath.EndsWith(".disabled"))
            {
                // Try to see if the user meant a disabled file but didn't type extension
                if (!File.Exists(filePath) && File.Exists(filePath + ".disabled"))
                {
                    filePath += ".disabled";
                    enabledPath = filePath.Substring(0, filePath.Length - 9);
                }
                else if (File.Exists(filePath))
                {
                    // Already enabled
                    return;
                }
            }

            if (File.Exists(filePath))
            {
                if (File.Exists(enabledPath)) File.Delete(enabledPath);
                File.Move(filePath, enabledPath);
            }
            else
            {
                throw new Exception($"Disabled mod '{fileName}' not found.");
            }
        }

        public static (List<Mod> mods, List<string> printLines) SearchMods(string instanceName, string query)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string instancePath = Path.Combine(workspaceDir, "instances", instanceName);
            string dataFile = Path.Combine(instancePath, "LegacyTUI_data");

            if (!File.Exists(dataFile))
            {
                // Fallback or error?
                // If no instance, one can't search compatible mods easily without args.
                // But requirements say: -i "my_instance" -m -g
                throw new Exception($"Instance '{instanceName}' not found or invalid.");
            }

            string[] data = File.ReadAllLines(dataFile);
            if (data.Length < 2)
            {
                throw new Exception($"Instance '{instanceName}' data corrupted.");
            }
            string version = data[0];
            string tag = data[1];

            var (pages, lines) = LegacyTUI.resManager.mods.global.Methods.search(version, tag, query);
            return (
                pages.SelectMany(x => x).ToList(),
                lines.SelectMany(x => x).ToList()
            );
        }
    }
}
