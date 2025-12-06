using LegacyTUIComp;
using LegacyTUIComp.Instances;

namespace LegacyTUIComp.Logic
{
    public class InstanceLogic
    {
        public static void CreateInstance(string name, string version, string tag)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string instancesPath = Path.Combine(workspaceDir, "instances");
            string instancePath = Path.Combine(instancesPath, name);

            if (Directory.Exists(instancePath))
            {
                throw new Exception($"Instance '{name}' already exists.");
            }

            Directory.CreateDirectory(instancePath);
            Methods.SetFromFile(Path.Combine(instancePath, "LegacyTUI_data"), new string[] { version, tag });
            Methods.updateProfiles(workspaceDir, instancePath);
        }

        public static void DeleteInstance(string name)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string instancePath = Path.Combine(workspaceDir, "instances", name);

            if (!Directory.Exists(instancePath))
            {
                throw new Exception($"Instance '{name}' does not exist.");
            }
            Directory.Delete(instancePath, true);
        }

        public static void LaunchInstance(string name, bool runInstant)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string instancePath = Path.Combine(workspaceDir, "instances", name);

            if (!Directory.Exists(instancePath))
            {
                throw new Exception($"Instance '{name}' does not exist.");
            }

            string dataFile = Path.Combine(instancePath, "LegacyTUI_data");
            if (!File.Exists(dataFile))
            {
                throw new Exception($"Instance '{name}' data not found.");
            }

            string[] data = File.ReadAllLines(dataFile);
            if (data.Length < 1)
            {
                throw new Exception($"Instance '{name}' data is corrupted.");
            }
            string version = data[0];

            LegacyTUIComp.Instances.Methods.runMC(instancePath, version, runInstant);
        }

        public static void RenameInstance(string currentName, string newName)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string currentPath = Path.Combine(workspaceDir, "instances", currentName);
            string newPath = Path.Combine(workspaceDir, "instances", newName);

            if (!Directory.Exists(currentPath))
            {
                throw new Exception($"Instance '{currentName}' does not exist.");
            }
            if (Directory.Exists(newPath))
            {
                throw new Exception($"Instance '{newName}' already exists.");
            }

            Directory.Move(currentPath, newPath);
            // Update logic if needed inside the instance (unlikely for simple folder rename)
        }

        public static void UpdateProfiles(string name)
        {
            string workspaceDir = Methods.WorkspaceDir();
            string instancePath = Path.Combine(workspaceDir, "instances", name);
            if (!Directory.Exists(instancePath))
            {
                throw new Exception($"Instance '{name}' does not exist.");
            }
            Methods.updateProfiles(workspaceDir, instancePath);
        }
    }
}
