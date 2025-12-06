namespace LegacyTUIComp
{
    public partial class Methods
    {
        public static void updateProfiles(string workspaceDir, string instancePath)
        {
            string launcherProfilesPath = Path.Combine(workspaceDir, "launcher_profiles.json");
            string tlauncherProfilesPath = Path.Combine(workspaceDir, "tlauncher_profiles.json");

            if (File.Exists(launcherProfilesPath) && File.Exists(tlauncherProfilesPath))
            {
                File.Copy(launcherProfilesPath, Path.Combine(instancePath, "launcher_profiles.json"), true);
                File.Copy(tlauncherProfilesPath, Path.Combine(instancePath, "tlauncher_profiles.json"), true);
            }
        }
    }
}