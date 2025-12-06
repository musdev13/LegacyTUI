namespace LegacyTUI.resManager.mods
{
    public partial class UI
    {
        public static void mod(string modName, string modsFolder)
        {
            Console.Clear();
            bool enabled = modName.Contains(".disabled") ? false : true;

            while (true)
            {
                Console.Clear();
                LegacyTUIComp.UI.showOptions(
                    modName,
                    new string[]
                    {
                        enabled ? "Disable" : "Enable",
                        "",
                        "",
                        "Delete"
                    },
                    "Back"
                );
                char choice = LegacyTUIComp.UI.getChar();

                switch (choice)
                {
                    case '0':
                        return;
                    case '1':
                        if (!enabled)
                        {
                            string newModName = modName.Replace(".disabled", "");
                            File.Move(Path.Combine(modsFolder, modName), Path.Combine(modsFolder, newModName));
                            enabled = true;
                            modName = newModName;
                        }
                        else
                        {
                            string newModName = modName + ".disabled";
                            File.Move(Path.Combine(modsFolder, modName), Path.Combine(modsFolder, newModName));
                            enabled = false;
                            modName = newModName;
                        }
                        break;
                    case '4':
                        Console.Clear();
                        Console.Write($"Are you sure about delete \"{modName}\" mod?\n>_ (y/n):");
                        char innerChoice = LegacyTUIComp.UI.getChar();

                        if (innerChoice == 'y')
                        {
                            File.Delete(Path.Combine(modsFolder, modName));
                            return;
                        }
                        break;
                }
            }
        }
    }
}