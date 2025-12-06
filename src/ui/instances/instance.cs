namespace LegacyTUIComp.Instances
{
    public partial class UI
    {
        public static void Instance(string instanceName)
        {
            string instancePath = Path.Combine(Path.Combine(LegacyTUIComp.Methods.WorkspaceDir(), "instances"), instanceName);
            string instanceVersion = LegacyTUIComp.Methods.GetFromFile(Path.Combine(instancePath, "LegacyTUI_data"), 0)!;
            string instanceTag = LegacyTUIComp.Methods.GetFromFile(Path.Combine(instancePath, "LegacyTUI_data"), 1)!;

            while (true)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions($"{instanceName}\nversion: {instanceVersion}", new string[] { "Run Instant", "Run Launcher (Recommended for first run)", "Resource Manager (Mods,RPacks,etc.)", "Change name", "Update Profiles", "", "Delete instance" }, "Back");
                char choice = LegacyTUIComp.UI.getChar();

                switch (choice)
                {
                    case '0':
                        return;
                    case '1':
                        LegacyTUIComp.Instances.Methods.runMC(instancePath, instanceVersion!, true);
                        break;
                    case '2':
                        LegacyTUIComp.Instances.Methods.runMC(instancePath, instanceVersion!, false);
                        break;
                    case '3':
                        LegacyTUI.resManager.UI.main(instancePath, instanceVersion, instanceTag);
                        break;
                    case '4':
                        LegacyTUIComp.Instances.Methods.changeName(instancePath);
                        return;
                    case '5':
                        Console.Clear();
                        Console.WriteLine("Updating profiles...");
                        // LegacyTUIComp.UI.getChar();
                        LegacyTUIComp.Methods.updateProfiles(LegacyTUIComp.Methods.WorkspaceDir(), instancePath);
                        break;
                    case '7':
                        if (LegacyTUIComp.Instances.Methods.deleteInstance(instanceName, instancePath)) return;
                        else break;

                }
            }

        }
    }
}
